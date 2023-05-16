
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Scenery", menuName = "ScriptableObject/Scenery")]
public class Scenery : ScriptableObject
{
    public Sprite sprite;
    public int spriteDensityMin; //how many will occur on a tile
    public int spriteDensityMax;
    public float isClumping; //will be used to decide if this tends to occcur by itself or in groups
    public bool isImpassable;
    public float sizeVariability;


    public float getImageWidth()
    {
        float answer = sprite.bounds.size.x;
        return answer;
    }

    public virtual void RightClick(PointerEventData eventData)
    {
        Debug.Log("Scenery object right clicked on");
    }
}
