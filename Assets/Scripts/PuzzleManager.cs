using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    
    
    [HideInInspector]
    public Scenery_Puzzle[] scenery_Puzzles = new Scenery_Puzzle[3];
    

    [HideInInspector]
    public Vector3Int portalLocation;
    [HideInInspector]
    public WorldScenery[] altars = new WorldScenery[3];
    [HideInInspector]
    public int altarsSolved = 0;


    private void Awake()
    {
        instance = this;
        
    }

    public void openPortal()
    {
     
            Debug.Log("Puzzle Manager triggers Portal");
     
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
