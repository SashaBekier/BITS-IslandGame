using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "Scenery_Puzzles", menuName = "ScriptableObject/Puzzle Scenery")]
public class Scenery_Puzzle : Scenery
{
    public Item_PuzzlePiece[] puzzlePieces;
    public Sprite[] bookAddedSprites = new Sprite[3];


    public override async void RightClick(PointerEventData eventData)
    {
        
        GameObject worldScenery = eventData.pointerPress;
        string worldSceneryName = worldScenery.name;
        
        WorldScenery altar = PuzzleManager.instance.getAltarObject(worldSceneryName);

        Debug.Log("Puzzle Scenery object right clicked on " + worldScenery);
        Vector3Int altarPos = PathFinder.instance.impassable.WorldToCell(altar.transform.position);
        List<Vector3Int> altarNeighbours = PathFinder.instance.getNeighbourCoords(altarPos);
        Vector3Int targetCoords = new Vector3Int(0,0,0);
        bool onNeighbour = false;
        bool abort = true;
        foreach (Vector3Int neighbourCoord in altarNeighbours)
        {
            if(PathFinder.instance.initData.impassableTilemap.WorldToCell(PathFinder.instance.initData.player.transform.position)  == neighbourCoord)
            {
                onNeighbour = true;
                abort = false;
                break;
            }
        }
        if (!onNeighbour)
        {
            foreach (Vector3Int neighbourCoord in altarNeighbours)
            {
                if (!PathFinder.instance.impassable.HasTile(neighbourCoord))
                {
                    PathFinder.instance.initData.player.GetComponent<PlayerMouseMovement>().enqueuePathToCoords(neighbourCoord);
                    targetCoords = neighbourCoord;
                    break;
                }
            }
            int counter = 0;
            
            while (PathFinder.instance.initData.impassableTilemap.WorldToCell(PathFinder.instance.initData.player.transform.position) != targetCoords && counter < 50)
            {
                await Task.Delay(100);
                counter++;
            }
            if (counter < 49)
            {
                abort = false;
            }
        }
        if (altar.needsBook && !abort)
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
                        GameObject.Find("Warrior").GetComponent<PlayerStats>().AdjustXP(500);
                    }
                    break;
                }
            }
        }
    }

}
