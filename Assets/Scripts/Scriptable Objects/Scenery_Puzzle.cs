using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Scenery_Puzzles", menuName = "ScriptableObject/Puzzle Scenery")]
public class Scenery_Puzzle : Scenery
{
    public Item_PuzzlePiece[] puzzlePieces;
    public Sprite[] bookAddedSprites = new Sprite[3];


    public override void RightClick(PointerEventData eventData)
    {
        
        GameObject worldScenery = eventData.pointerPress;
        string worldSceneryName = worldScenery.name;
        
        WorldScenery altar = PuzzleManager.instance.getAltarObject(worldSceneryName);

        Debug.Log("Puzzle Scenery object right clicked on " + worldScenery);
        
        if (altar.needsBook)
        {
            Debug.Log("The altar is empty");
            for (int i = 0; i < puzzlePieces.Length; i++)
            {
                if (InventoryManager.instance.hasItemInInventory(puzzlePieces[i]))
                {
                    Debug.Log(puzzlePieces[i] + "detected in inventory");
                    InventoryManager.instance.DestroyItem(puzzlePieces[i]);
                    altar.setSprite(bookAddedSprites[i]);
                    altar.needsBook = false;
                    PuzzleManager.instance.altarsSolved++;
                    Debug.Log("Puzzle Manager reports " + PuzzleManager.instance.altarsSolved + " Altars solved");
                    if(PuzzleManager.instance.altarsSolved == 3)
                    {
                        PuzzleManager.instance.openPortal();
                    }
                    break;
                }
            }
        }
    }

}
