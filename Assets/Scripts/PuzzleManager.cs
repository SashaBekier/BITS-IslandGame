using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    
    
    [HideInInspector]
    public Scenery_Puzzle[] scenery_Puzzles = new Scenery_Puzzle[3];
    

    [HideInInspector]
    public Vector3Int portalLocation;
    public Scenery portal;
    [HideInInspector]
    public WorldScenery[] altars = new WorldScenery[3];
    [HideInInspector]
    public int altarsSolved = 0;

    public WorldScenery sceneryPrefab;


    private void Awake()
    {
        instance = this;
        
    }

    public void openPortal()
    {
     
            Debug.Log("Puzzle Manager triggers Portal");
        //GameObject.Instantiate(portalPrefab, PathFinder.instance.impassable.CellToWorld(portalLocation), Quaternion.identity);
        Vector3 portalPosition = PathFinder.instance.impassable.CellToWorld(portalLocation);
        WorldScenery newScenery = Instantiate(sceneryPrefab, portalPosition, Quaternion.identity);
        WorldScenery worldScenery = newScenery.GetComponent<WorldScenery>();
        worldScenery.Initialise(portal);
        portalPosition = PathFinder.instance.impassable.CellToWorld(new Vector3Int(-15,8,0));
        newScenery = Instantiate(sceneryPrefab, portalPosition, Quaternion.identity);
        worldScenery = newScenery.GetComponent<WorldScenery>();
        worldScenery.Initialise(portal);
    }

    public WorldScenery getAltarObject(string name)
    {
        Debug.Log("Puzzle manager seeking " + name);
        WorldScenery thisScenery = altars[0];
        for (int i = 0; i < altars.Length; i++)
        {
            Debug.Log("Checking against " + altars[i].name);
            if (altars[i].name.Equals(name))
            {
                Debug.Log("Hit");
                thisScenery = altars[i];
                break;
            } else
            {
                Debug.Log("Miss");
            }
        }
        return thisScenery;
    }
}
