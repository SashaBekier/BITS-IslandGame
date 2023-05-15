using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Scenery_Puzzles", menuName = "ScriptableObject/Puzzle Scenery")]
public class Scenery_Puzzle : Scenery
{
    public Item_PuzzlePiece[] puzzlePieces;
    public override void RightClick()
    {
        Debug.Log("Puzzle Scenery object right clicked on");
        for(int i = 0; i < puzzlePieces.Length; i++)
        {
            Debug.Log("Checking for Puzzle Piece " + puzzlePieces[i]);
            if (InventoryManager.instance.HasItemInInventory(puzzlePieces[i]))
            {
                Debug.Log("Puzzle Piece found");
            } else
            {
                Debug.Log("Puzzle piece not found");
            }
        }
        
    }

}
