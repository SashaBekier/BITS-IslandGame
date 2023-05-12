using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;
using static UnityEngine.Random;


public class initialiseMap : MonoBehaviour
{
    private PathFinder finder;

    public GameObject player;

    public int islandSize = 50;
    
    public Tilemap fogTilemap;
    public Tilemap impassableTilemap;
    public Tilemap terrainTilemap;

    List<Vector3Int> availableCoords = new List<Vector3Int>();

    public Tile oceanTile;

    public Tile rockTile;
    public Tile fogTile;
    public Tile collisionTile;


    public Tile[] puzzleTiles;
    public Tile[] groundTiles;

    private int puzzleReference = 0;
    private int maxOceanWidth = 15;
    public WorldScenery sceneryPrefab;
    public Pickupable worldItemPrefab;
    public Item[] items;
    public Item_Edible[] edibles;
    public Item_PuzzlePiece[] puzzlePieces;

    public Scenery[] plants;

    public Scenery[] puzzleReceivers;
    public Scenery[] rocks;




    private Vector3 playerShim = new Vector3(0f, .4f, 0f);



    // Start is called before the first frame update
    void Start()
    {
        finder = PathFinder.instance;
        //UnityEngine.Random.InitState(16726);

        //initialise tile randomisation arrays
        /*
        puzzleTiles[0] = puzzleTile1;
        puzzleTiles[1] = puzzleTile2;
        puzzleTiles[2] = puzzleTile3;
        groundTiles[0] = groundTile1;
        groundTiles[1] = groundTile2;
        groundTiles[2] = groundTile3;
*/
        initialiseAvailableTiles();

        

        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        CreateIsland();

        FixSingleTileIslands();
        SetOceanTones();
        
        

        SpawnEdibles();
        SpawnPlants();
        
        SpawnRocks();
        PlacePlayer();
        SpawnPuzzlePieces();

    }

    private void PlacePlayer()
    {
        Vector3Int gridPosition1;
        do {
            gridPosition1 = getTargetTile();
            Vector3 worldPos = terrainTilemap.CellToWorld(gridPosition1);
            player.transform.position = worldPos - playerShim;
        } while (!accessibleToPlayer(new Vector3Int(islandSize / 2, islandSize / 2)));
        setCoordsUnavailable(gridPosition1);
    }

    private void SpawnRocks()
    {
        for (int i = 0; i < 250; i++)
        {
            UnityEngine.Debug.Log("Spawning Rocks");
            Vector3Int gridPosition1 = getTargetTile();
            int rockIndex = UnityEngine.Random.Range(0, rocks.Length);
            
            Vector3 rockPosition = terrainTilemap.CellToWorld(gridPosition1);
            if (rocks[rockIndex].isImpassable)
            {
                impassableTilemap.SetTile(gridPosition1, collisionTile);
            }

            WorldScenery newScenery = Instantiate(sceneryPrefab, rockPosition, Quaternion.identity);
            WorldScenery worldScenery = newScenery.GetComponent<WorldScenery>();
            worldScenery.Initialise(rocks[rockIndex]);
            float scaleMod = UnityEngine.Random.Range(1 - worldScenery.scenery.sizeVariability, 1 + worldScenery.scenery.sizeVariability);
            worldScenery.transform.localScale = new Vector3(scaleMod, scaleMod);
            setCoordsUnavailable(gridPosition1);


        }
    }

    private void initialiseAvailableTiles()
    {
        for (int i = 0; i < islandSize; i++)
        {
            for(int j=0; j < islandSize; j++)
            {
                availableCoords.Add(new Vector3Int(i, j));
            }
        }
        setCoordsUnavailable(new Vector3Int(islandSize / 2, islandSize / 2)); 
    }

    private bool coordsAreAvailable(Vector3Int coords)
    {
        if (availableCoords.Contains(coords))
        {
            return true;
        }
        return false;
    }

    private void setCoordsUnavailable(Vector3Int coords)
    {
        if (availableCoords.Contains(coords))
        {
            availableCoords.Remove(coords);
        }
    }

    private void SpawnEdibles()
    {
        for (int i = 0; i < 20; i++)
        {
            UnityEngine.Debug.Log("Spawning Edibles");
            Vector3Int gridPosition1 = getTargetTile();
            Vector3 appleposition = terrainTilemap.CellToWorld(gridPosition1);

            Pickupable newItem = Instantiate(worldItemPrefab, appleposition, Quaternion.identity);
            Pickupable worldItem = newItem.GetComponent<Pickupable>();
            worldItem.Initialise(edibles[UnityEngine.Random.Range(0,edibles.Length)]);
            setCoordsUnavailable(gridPosition1);
                

        }
    }

    private Vector3Int getTargetTile()
    {
        Vector3Int gridPosition1 = availableCoords[UnityEngine.Random.Range(0,availableCoords.Count)];
        return gridPosition1;
        
    }

    private void SpawnPuzzlePieces()
    {
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            Vector3Int gridPosition1;
            
            UnityEngine.Debug.Log("Spawning Books");
            
            do {
                gridPosition1 = getTargetTile();
                
            } while (!accessibleToPlayer(gridPosition1));
            Vector3 bookposition = terrainTilemap.CellToWorld(gridPosition1);
            Pickupable newItem = Instantiate(worldItemPrefab, bookposition, Quaternion.identity);
            Pickupable worldItem = newItem.GetComponent<Pickupable>();
            worldItem.Initialise(puzzlePieces[i]);
            setCoordsUnavailable(gridPosition1);

        }
    }

    private bool accessibleToPlayer(Vector3Int coords)
    {
        Vector3Int playerPosition = terrainTilemap.WorldToCell(player.transform.position - playerShim );
        Dictionary<Vector3Int, int>  steps = finder.GenerateStepCount(playerPosition);
        return steps.ContainsKey(coords);
    }

    private void SpawnPlants()
    {
        for (int i = 0; i < 30; i++)
        {
            UnityEngine.Debug.Log("Spawning Plants");
            Vector3Int gridPosition1 = getTargetTile();
            int plantIndex = UnityEngine.Random.Range(0, plants.Length);
            int plantDensity = UnityEngine.Random.Range(plants[plantIndex].spriteDensityMin, plants[plantIndex].spriteDensityMax+1);
            for (int j = 0; j < plantDensity; j++)
            {
                bool getNeighbour = false;
                do
                {
                    SpawnPlant(gridPosition1, plantIndex, getNeighbour);
                    getNeighbour = true;
                } while (UnityEngine.Random.Range(0f, 1f) < plants[plantIndex].isClumping);
            }

        }
    }

    private void SpawnPlant(Vector3Int gridPosition1, int plantIndex, bool shiftToNeighbour)
    {
        if (shiftToNeighbour)
        {
            List<Vector3Int> toRemove = new List<Vector3Int>();
            List<Vector3Int> neighbours = finder.getNeighbourCoords(gridPosition1);
            foreach (Vector3Int neighbour in neighbours)
            {
                if (!coordsAreAvailable(neighbour)) // TODO fix with avaiulableTiles List
                {
                    toRemove.Add(neighbour);
                }
            }
            foreach(Vector3Int coords in toRemove)
            {
                neighbours.Remove(coords);
            }
            if (neighbours.Count > 0)
            {
                gridPosition1 = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
            }

        }
        
        Vector3 plantPosition = offsetPositionWithinCell(gridPosition1);
        if (plants[plantIndex].isImpassable)
        {
            impassableTilemap.SetTile(gridPosition1, collisionTile);
        }
        
        WorldScenery newScenery = Instantiate(sceneryPrefab, plantPosition, Quaternion.identity);
        WorldScenery worldScenery = newScenery.GetComponent<WorldScenery>();
        worldScenery.Initialise(plants[plantIndex]);
        float scaleMod = UnityEngine.Random.Range(1 - worldScenery.scenery.sizeVariability, 1 + worldScenery.scenery.sizeVariability);
        worldScenery.transform.localScale = new Vector3(scaleMod, scaleMod);
        setCoordsUnavailable(gridPosition1);
    }

    public Vector3 offsetPositionWithinCell(Vector3Int cellPosition, float yOffset)
    {
        Vector3 plantPosition = terrainTilemap.CellToWorld(cellPosition);
        plantPosition += new Vector3(UnityEngine.Random.Range(-.25f, +.25f), UnityEngine.Random.Range(-.25f + yOffset, +.25f + yOffset), 0);
        return plantPosition;
    }

    public Vector3 offsetPositionWithinCell(Vector3Int cellPosition)
    {
        return offsetPositionWithinCell(cellPosition, .25f);
    }

    void FixSingleTileIslands()
    {
        Dictionary<Vector3Int, int> steps = finder.GenerateStepCount(new Vector3Int(islandSize / 2, islandSize / 2));
        for (int xcoord = 0; xcoord <= islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= islandSize; ycoord++)
            {
                Vector3Int coords = new Vector3Int(xcoord, ycoord);
                Debug.Log(coords + " " +terrainTilemap.GetTile(coords).ToString());
                if (!terrainTilemap.GetTile(coords).ToString().Equals("ocean_tile"))
                {
                    
                    if (!steps.ContainsKey(coords))
                    {
                                                
                            setOceanTile(coords);
                        
                    }
                }
          }
        }
    }

    void SetOceanTones()
    {
        for (int xcoord = 0; xcoord <= islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= islandSize; ycoord++)
            {
                Vector3Int tileCoords = new Vector3Int(xcoord, ycoord, 0);
                
                if (terrainTilemap.GetTile<Tile>(tileCoords) == oceanTile)
                {
                    if (hasNeighbouringLand(tileCoords) == 0)
                    {
                        List<Vector3Int> neighbours = finder.getNeighbourCoords(tileCoords);
                        bool isDeeps = true;
                        foreach (Vector3Int neighbour in neighbours)
                        {
                            if (hasNeighbouringLand(neighbour) > 0)
                            {
                                isDeeps = false;
                            }
                        }
                        terrainTilemap.SetTileFlags(tileCoords, TileFlags.None);
                        if (isDeeps)
                        {
                            terrainTilemap.SetColor(tileCoords, new Color(0.1f, .2155f, .9f, 1f));
                        } else
                        {
                            terrainTilemap.SetColor(tileCoords, new Color(0f, .6f, .8f, 1f));
                        }
                    }
                }
            }
        }
    }

    void CreateIsland()
    {
        Dictionary<Vector3Int, int> tiles = new Dictionary<Vector3Int, int>();
        for (int xcoord = 0; xcoord <= islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= islandSize; ycoord++)
            {
                Vector3Int tileCoords = new Vector3Int(xcoord, ycoord, 0);
                fogTilemap.SetTile(tileCoords, fogTile);
                int distance = distanceFromEdge(tileCoords);
                

                if (xcoord == 0 || ycoord == 0 || xcoord == islandSize || ycoord == islandSize)
                {
                    setOceanTile(tileCoords);
                } else
                {
                    tiles.Add(tileCoords, distance);    
                }
                
            }
        }
        foreach(KeyValuePair<Vector3Int, int> tile in tiles.OrderBy(key => key.Value))
        {
            if(tile.Value < maxOceanWidth)
            {
                if(hasNeighbouringOcean(tile.Key)>0)
                {
                    if(UnityEngine.Random.Range(0,maxOceanWidth) > tile.Value)
                    {
                        setOceanTile(tile.Key);
                    }
                }
            }
            if (!terrainTilemap.HasTile(tile.Key))
            {
                setLandTile(tile.Key);
            }
        }
    }

    int distanceFromEdge(Vector3Int coords)
    {
        int distance = 100;
        if(coords.x < distance) distance = coords.x;
        if(coords.y < distance) distance = coords.y;
        if (islandSize - coords.x < distance) distance = islandSize - coords.x;
        if (islandSize - coords.y < distance) distance = islandSize - coords.y;
        //UnityEngine.Debug.Log(distance.ToString());
        return distance;
    }

    int hasNeighbouringOcean(Vector3Int tileCoords)
    {
        int tileX = tileCoords.x;
        int tileY = tileCoords.y;

        int neighbourCount = 0;

        Vector3Int[] neighbours = new Vector3Int[6];
        neighbours[0] = new Vector3Int(tileX+1, tileY, 0);
        neighbours[1] = new Vector3Int(tileX-1, tileY, 0);


        if (tileY % 2 == 0)
        {
            tileX--;
        }
        neighbours[2] = new Vector3Int(tileX + 1, tileY + 1, 0);
        neighbours[3] = new Vector3Int(tileX, tileY + 1, 0);
        neighbours[4] = new Vector3Int(tileX + 1, tileY - 1, 0);
        neighbours[5] = new Vector3Int(tileX, tileY - 1, 0);

        foreach(Vector3Int neighbour in neighbours)
        {
            if (terrainTilemap.GetTile<Tile>(neighbour)==oceanTile)
            {
                neighbourCount++;
            }
        }

        return neighbourCount;
    }

    int hasNeighbouringLand(Vector3Int tileCoords)
    {
        int tileX = tileCoords.x;
        int tileY = tileCoords.y;

        int neighbourCount = 0;

        Vector3Int[] neighbours = new Vector3Int[6];
        neighbours[0] = new Vector3Int(tileX + 1, tileY, 0);
        neighbours[1] = new Vector3Int(tileX - 1, tileY, 0);


        if (tileY % 2 == 0)
        {
            tileX--;
        }
        neighbours[2] = new Vector3Int(tileX + 1, tileY + 1, 0);
        neighbours[3] = new Vector3Int(tileX, tileY + 1, 0);
        neighbours[4] = new Vector3Int(tileX + 1, tileY - 1, 0);
        neighbours[5] = new Vector3Int(tileX, tileY - 1, 0);

        foreach (Vector3Int neighbour in neighbours)
        {
            foreach (Tile landTile in groundTiles)
            {
                if (terrainTilemap.GetTile<Tile>(neighbour) == landTile)
                {
                    neighbourCount++;
                }
            }
        }

        return neighbourCount;
    }

    void setOceanTile(Vector3Int tileCoords)
    {
        terrainTilemap.SetTile(tileCoords, oceanTile);
        impassableTilemap.SetTile(tileCoords, collisionTile);
        setCoordsUnavailable(tileCoords);
    }

    void setLandTile(Vector3Int tileCoords)
    {
        terrainTilemap.SetTile(tileCoords, groundTiles[UnityEngine.Random.Range(0, groundTiles.Length)]);
    }

    void PlaceFixedGroupTiles()
    {
        int xOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize-maxOceanWidth);
        int yOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize - maxOceanWidth);


        if (yOffset % 2 != 0)
        {
            yOffset++;
        }
        Vector3Int gridPosition1 = new Vector3Int(xOffset, yOffset, 0);
        Vector3Int gridPosition2 = new Vector3Int(xOffset + 1, yOffset, 0);
        Vector3Int gridPosition3 = new Vector3Int(xOffset, yOffset + 1, 0);


        if (!terrainTilemap.HasTile(gridPosition1) && !terrainTilemap.HasTile(gridPosition2) && !terrainTilemap.HasTile(gridPosition3))
        {
            terrainTilemap.SetTile(gridPosition1, puzzleTiles[puzzleReference]);
            terrainTilemap.SetTile(gridPosition2, puzzleTiles[puzzleReference]);
            terrainTilemap.SetTile(gridPosition3, puzzleTiles[puzzleReference]);
            puzzleReference++;

        }
        else
        {
            PlaceFixedGroupTiles();
        }


    }




}
