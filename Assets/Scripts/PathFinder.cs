using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

using UnityEngine.Tilemaps;
using System.Runtime.CompilerServices;

public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;
    public initialiseMap initData;
    public Tilemap impassable;
    private void Awake()
    {
        instance = this;
    }
    private int CellToCellDistance(Vector3Int startCoords, Vector3Int endCoords)
    {
        int answer = 10000;
        int yDistance = Mathf.Abs(startCoords.y - endCoords.y);
        int freeXMin = startCoords.x - (yDistance / 2);
        if(startCoords.y % 2 == 0 && yDistance%2 == 1 )
        {
            freeXMin--;
        }
        int freeXMax = freeXMin + yDistance;
        if(endCoords.x >= freeXMin && endCoords.x <= freeXMax) {
            answer = yDistance;
        } else if (endCoords.x < freeXMin)
        {
            answer = freeXMin - endCoords.x + yDistance;
        } else
        {
            answer = endCoords.x - freeXMax + yDistance;
        }
        return answer;
        /* int dx = endCoords.x - startCoords.x;     // signed deltas
         int dy = endCoords.y - startCoords.y;
         int x = Mathf.Abs(dx);  // absolute deltas
         int y = Mathf.Abs(dy);
         // special case if we start on an odd row or if we move into negative x direction
         if ((dx < 0) || ((startCoords.y % 2) == 1))
             x = Mathf.Max(0, x - (y + 1) / 2);
         else
             x = Mathf.Max(0, x - (y) / 2);
         return x + y;*/
        //return 0;
    }

    public Queue<Vector3Int> FindPath(Vector3Int startCoords, Vector3Int endCoords)
    {
        Queue<Vector3Int> path = new Queue<Vector3Int>();
        path.Enqueue(startCoords);

        if (impassable.HasTile(endCoords))
        {
            return path;
        }
        Dictionary<Vector3Int, int> outboundTiles = getDistanceMappedTiles(startCoords);
        Dictionary<Vector3Int, int> inboundTiles = getDistanceMappedTiles(endCoords);
        Dictionary<Vector3Int, int> outboundSteps = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> inboundSteps = new Dictionary<Vector3Int, int>();
        Dictionary<Vector3Int, int> combinedSteps = new Dictionary<Vector3Int, int>();
        GenerateStepCount(outboundTiles, outboundSteps);
        GenerateStepCount(inboundTiles, inboundSteps);
        int lowestPath = 10000;
        for (int i = 0; i < initData.islandSize; i++)
        {
            for (int j = 0; j < initData.islandSize; j++)
            {
                
                Vector3Int setTileCoords = new Vector3Int(i, j, 0);
                combinedSteps[setTileCoords] = outboundSteps[setTileCoords] + inboundSteps[setTileCoords];
                if (combinedSteps[setTileCoords] <= lowestPath)
                {
                    lowestPath = combinedSteps[setTileCoords];
                    
                }
            }
        }
        
        Vector3Int currentCoords = startCoords;
        Vector3Int lastCoords = startCoords;
        int stepsToGo = inboundSteps[startCoords];
        int steps = 0;
        Debug.Log("Start " + startCoords.x + ", " + startCoords.y);
        Debug.Log("End " + endCoords.x + ", " + endCoords.y);
        while (!currentCoords.Equals(endCoords) && steps < 10)
        {
            steps++;
            
            List<Vector3Int> neighbours = getNeighbourCoords(currentCoords);
            for(int i = 0;i < neighbours.Count; i++)
            {
                if (combinedSteps[neighbours[i]] == lowestPath && inboundSteps[neighbours[i]] < stepsToGo)
                {
                    path.Enqueue(neighbours[i]);
                    stepsToGo--;
                    Debug.Log("Enqueuing " + neighbours[i].x + ", " + neighbours[i].y);
                    
                    currentCoords = neighbours[i];
                    i = 8;
                }
            }
        }
        
        return path;
    }

    private void GenerateStepCount(Dictionary<Vector3Int, int> distanceMappedTiles, Dictionary<Vector3Int, int> stepCount)
    {
        foreach (KeyValuePair<Vector3Int, int> tile in distanceMappedTiles.OrderBy(key => key.Value))
        {
            if (impassable.HasTile(tile.Key))
            {
                stepCount.Add(tile.Key, 10000);
            }
            else
            {
                if (tile.Value == 0)
                {
                    stepCount.Add(tile.Key, tile.Value);
                }
                else
                {
                    stepCount.Add(tile.Key, getLowestNeighbourValue(tile.Key, stepCount) + 1);
                }
            }
            if (stepCount[tile.Key] <= 1)
            {
                Debug.Log("For " + tile.Key.x + "," + tile.Key.y + " step count recorded " + stepCount[tile.Key]);
            }
        }
    }

    private int getLowestNeighbourValue(Vector3Int coords, Dictionary<Vector3Int, int> stepCount)
    {
        int answer = 10000;
        List<Vector3Int> neighbours = getNeighbourCoords(coords);
        foreach (Vector3Int neighbour in neighbours)
        {
            if (stepCount.ContainsKey(neighbour))
            {
                if(answer > stepCount[neighbour])
                {
                    answer = stepCount[neighbour];
                }
            }
        }
        return answer;
    }

    private List<Vector3Int> getNeighbourCoords(Vector3Int coords)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        if (coords.x > 0)
        {
            neighbours.Add(new Vector3Int(coords.x - 1, coords.y));
        }
        if (coords.x < initData.islandSize)
        {
            neighbours.Add(new Vector3Int(coords.x + 1, coords.y));
        }
        if (coords.y % 2 == 0)
        {
            coords.x--;
        }
        if (coords.y > 0 && coords.x > 0)
        {
            neighbours.Add(new Vector3Int(coords.x, coords.y + 1));
        }
        if (coords.y > 0 && coords.x < initData.islandSize)
        {
            neighbours.Add(new Vector3Int(coords.x+1, coords.y + 1));
        }
        if (coords.y < initData.islandSize && coords.x > 0)
        {
            neighbours.Add(new Vector3Int(coords.x, coords.y - 1));
        }
        if (coords.y < initData.islandSize && coords.x < initData.islandSize)
        {
            neighbours.Add(new Vector3Int(coords.x + 1, coords.y - 1));
        }
        return neighbours;

    }

    private Dictionary<Vector3Int, int> getDistanceMappedTiles(Vector3Int referenceTile)
    {
        Dictionary<Vector3Int, int> tiles = new Dictionary<Vector3Int, int>();
        for (int xcoord = 0; xcoord <= initData.islandSize; xcoord++)
        {
            for (int ycoord = 0; ycoord <= initData.islandSize; ycoord++)
            {
                Vector3Int tileCoords = new Vector3Int(xcoord, ycoord, 0);
                int distance = CellToCellDistance(tileCoords, referenceTile);
                tiles.Add(tileCoords, distance);
            }
        }
        return tiles;
    }
}
