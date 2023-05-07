using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using UnityEngine;


using UnityEngine.Tilemaps;


public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;
    public initialiseMap initData;
    public Tilemap impassable;
    private void Awake()
    {
        instance = this;
    }
   

    public Queue<Vector3Int> FindPath(Vector3Int startCoords, Vector3Int endCoords)
    {
        Queue<Vector3Int> path = new Queue<Vector3Int>();
        Vector3Int lastEnqueued = startCoords;
        
        Dictionary<Vector3Int, int> outboundStepCount = GenerateStepCount(startCoords);
        if (outboundStepCount.ContainsKey(endCoords)) 
        {
            Dictionary<Vector3Int, int> inboundStepCount = GenerateStepCount(endCoords);
            Dictionary<Vector3Int, int> combinedStepCount = new Dictionary<Vector3Int, int>();
            int lowestCombined = 10000;
            for (int i = 0; i < initData.islandSize; i++)
            {
                for (int j = 0; j < initData.islandSize; j++)
                {
                    Vector3Int coords = new Vector3Int(i, j, 0);

                    if (inboundStepCount.ContainsKey(coords) && outboundStepCount.ContainsKey(coords))
                    {
                        combinedStepCount.Add(coords, outboundStepCount[coords] + inboundStepCount[coords]);
                        if (combinedStepCount[coords] < lowestCombined)
                        {
                            lowestCombined = combinedStepCount[coords];
                        }
                    }
                }
            }
            int steps = inboundStepCount[startCoords] - 1;
            while(steps > 0)
            {
                foreach(KeyValuePair<Vector3Int,int> pair in combinedStepCount)
                {
                    bool neighbourCheck = areAdjacent(lastEnqueued, pair.Key);
                    if(pair.Value == lowestCombined && inboundStepCount[pair.Key] == steps && neighbourCheck) {
                        
                        path.Enqueue(pair.Key);
                        lastEnqueued = pair.Key;
                        break;
                    }
                }
                steps--;
            }
            path.Enqueue(endCoords);
        } 

        return path;
    }

    private bool areAdjacent(Vector3Int coords1, Vector3Int toCoords)
    {
        bool answer = false;
        List<Vector3Int> neighbours = getNeighbourCoords(coords1);
        foreach (Vector3Int neighbour in neighbours)
        {
            if (toCoords.Equals(neighbour))
            {
                answer = true;
            }
        }
        return answer;
    }
    private Dictionary<Vector3Int, int> GenerateStepCount(Vector3Int coords)
    {
        Queue<Vector3Int> checkNeighboursOf = new Queue<Vector3Int>();
        Dictionary<Vector3Int, int> answer = new Dictionary<Vector3Int, int>();

        
        answer.Add(coords, 0);
        
        checkNeighboursOf.Enqueue(coords);
        while(checkNeighboursOf.Count > 0 )
        {
            Vector3Int checkNeighbour = checkNeighboursOf.Dequeue();
            List<Vector3Int> newNeighbours = getNeighbourCoords(checkNeighbour);
            foreach(Vector3Int neighbour in newNeighbours)
            {
                
                if(!impassable.HasTile(neighbour) && !answer.ContainsKey(neighbour)) {
                    answer.Add(neighbour, answer[checkNeighbour] +1);
                    checkNeighboursOf.Enqueue(neighbour);
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

 
}
