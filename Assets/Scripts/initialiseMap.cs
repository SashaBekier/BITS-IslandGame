using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Random;


public class initialiseMap : MonoBehaviour
{
    public int islandSize = 50;
    public Tilemap fogTilemap;
    public Tilemap impassableTilemap;
    public Tilemap terrainTilemap;

    public Tile oceanTile;
    public Tile groundTile1;
    public Tile groundTile2;
    public Tile groundTile3;
    public Tile rockTile;
    public Tile fogTile;
    public Tile collisionTile;
    public Tile puzzleTile1;
    public Tile puzzleTile2;
    public Tile puzzleTile3;

    private static int groundTileVarietyCount = 3;
    private static int puzzleTileVarietyCount = 3;

    private Tile[] puzzleTiles = new Tile[puzzleTileVarietyCount];
    private Tile[] groundTiles = new Tile[groundTileVarietyCount];

    private int puzzleReference = 0;
    private int maxOceanWidth = 10;
    private float oceanSpawnChance = .6f;




    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Random.InitState(16726);
        
        //initialise tile randomisation arrays
        puzzleTiles[0] = puzzleTile1;
        puzzleTiles[1] = puzzleTile2;
        puzzleTiles[2] = puzzleTile3;
        groundTiles[0] = groundTile1;
        groundTiles[1] = groundTile2;
        groundTiles[2] = groundTile3;


        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        PlaceFixedGroupTiles();
        CreateIsland();



    }

    void CreateIsland()
    {
        for(int xcoord = 0; xcoord <= islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= islandSize; ycoord++)
            {
                Vector3Int thisTile = new Vector3Int(xcoord, ycoord, 0);
                fogTilemap.SetTile(thisTile, fogTile);
                if (xcoord == 0 || ycoord == 0 || xcoord == islandSize || ycoord == islandSize)
                {
                    setOceanTile(thisTile);
                } else
                {
                    if (!terrainTilemap.HasTile(thisTile))
                    {
                        if (xcoord < maxOceanWidth || ycoord < maxOceanWidth || xcoord > islandSize - maxOceanWidth || ycoord > islandSize-maxOceanWidth)
                        {
                            if (hasNeighbouringOcean(thisTile) && UnityEngine.Random.Range(0f, 1f) > oceanSpawnChance)
                            {
                                setOceanTile(thisTile);
                            }
                        }
                        if (!terrainTilemap.HasTile(thisTile))
                        {
                            setLandTile(thisTile);
                        }




                    }
                }
            }
        }
    }

    bool hasNeighbouringOcean(Vector3Int tileCoords)
    {
        //Vector3Int
        return true;
    }

    void setOceanTile(Vector3Int tileCoords)
    {
        terrainTilemap.SetTile(tileCoords, oceanTile);
        impassableTilemap.SetTile(tileCoords, collisionTile);
    }

    void setLandTile(Vector3Int tileCoords)
    {
        terrainTilemap.SetTile(tileCoords, groundTiles[UnityEngine.Random.Range(0, groundTileVarietyCount)]);
    }

    void PlaceFixedGroupTiles()
    {
        int xOffset = UnityEngine.Random.Range(10, 39);
        int yOffset = UnityEngine.Random.Range(10, 39);


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
