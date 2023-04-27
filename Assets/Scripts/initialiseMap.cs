using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Random;


public class initialiseMap : MonoBehaviour
{
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

    private Tile[] puzzleTiles = new Tile[3];
    private Tile[] groundTiles = new Tile[3];

    private int puzzleReference = 0;




    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Random.InitState(16726);
        //terrainTilemap.SetTile(new Vector3Int(0, 0, 0), groundTile);
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
        for(int xcoord = 0; xcoord < 50; xcoord++)
        {
            for (int ycoord = 0; ycoord < 50; ycoord++)
            {
                Vector3Int thisTile = new Vector3Int(xcoord, ycoord, 0);
                fogTilemap.SetTile(thisTile, fogTile);
                if (xcoord == 0 || ycoord == 0 || xcoord == 50 || ycoord == 50)
                {
                    terrainTilemap.SetTile(thisTile, oceanTile);
                    impassableTilemap.SetTile(thisTile, collisionTile);
                } else
                {
                    if (!terrainTilemap.HasTile(thisTile))
                    {
                        terrainTilemap.SetTile(thisTile, groundTiles[UnityEngine.Random.Range(0,3)]);
                    }
                }
            }
        }
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
