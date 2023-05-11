using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;
using static UnityEngine.Random;


public class initialiseMap : MonoBehaviour
{
    public int islandSize = 50;
    public Tilemap fogTilemap;
    public Tilemap impassableTilemap;
    public Tilemap terrainTilemap;

    public Tile oceanTile;

    public Tile rockTile;
    public Tile fogTile;
    public Tile collisionTile;


    public Tile[] puzzleTiles;
    public Tile[] groundTiles;

    private int puzzleReference = 0;
    private int maxOceanWidth = 15;

    public Item[] items;
    public Item_Edible[] edibles;
    public Pickupable worldItemPrefab;
    public Scenery[] plants;
    public WorldScenery sceneryPrefab;
    public Scenery[] puzzleReceivers;




    // Start is called before the first frame update
    void Start()
    {
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

        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        CreateIsland();

        FixSingleTileIslands();
        SetOceanTones();

        SpawnEdibles();
        SpawnPlants();

    }

    private void SpawnEdibles()
    {
        for (int i = 0; i < 20; i++)
        {
            UnityEngine.Debug.Log("Spawning Apples");
            int xOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize - maxOceanWidth);
            int yOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize - maxOceanWidth);
            Vector3Int gridPosition1 = new Vector3Int(xOffset, yOffset, 0);
            Vector3 appleposition = terrainTilemap.CellToWorld(gridPosition1);

            Pickupable newItem = Instantiate(worldItemPrefab, appleposition, Quaternion.identity);
            Pickupable worldItem = newItem.GetComponent<Pickupable>();
            worldItem.Initialise(edibles[0]);

        }
    }

    private void SpawnPlants()
    {
        for (int i = 0; i < 30; i++)
        {
            UnityEngine.Debug.Log("Spawning Plants");
            int xOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize - maxOceanWidth);
            int yOffset = UnityEngine.Random.Range(maxOceanWidth, islandSize - maxOceanWidth);
            Vector3Int gridPosition1 = new Vector3Int(xOffset, yOffset, 0);
            int plantIndex = UnityEngine.Random.Range(0, plants.Length);
            int plantDensity = UnityEngine.Random.Range(plants[plantIndex].spriteDensityMin, plants[plantIndex].spriteDensityMax+1);
            for (int j = 0; j < plantDensity; j++)
            {
                Vector3 plantPosition = offsetPositionWithinCell(gridPosition1);
                WorldScenery newScenery = Instantiate(sceneryPrefab, plantPosition, Quaternion.identity);
                WorldScenery worldScenery = newScenery.GetComponent<WorldScenery>();
                worldScenery.Initialise(plants[plantIndex]);
            }

        }
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
        for (int xcoord = 0; xcoord <= islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= islandSize; ycoord++)
            {
                Vector3Int tileCoords = new Vector3Int(xcoord, ycoord, 0);
                foreach (Tile landTile in groundTiles)
                {
                    if (terrainTilemap.GetTile<Tile>(tileCoords) == landTile)
                    {
                        if (hasNeighbouringLand(tileCoords) == 0)
                        {
                            setOceanTile(tileCoords);
                        }
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
                        terrainTilemap.SetTileFlags(tileCoords, TileFlags.None);
                        terrainTilemap.SetColor(tileCoords, new Color(0f, .4155f, 1f, 1f)); 
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
