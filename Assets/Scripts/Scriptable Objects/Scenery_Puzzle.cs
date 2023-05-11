using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Scenery_Puzzles", menuName = "ScriptableObject/Puzzle Scenery")]
public class Scenery_Puzzle : Scenery
{
    public override void RightClick()
    {
        Debug.Log("Puzzle Scenery object right clicked on");
    }

}
