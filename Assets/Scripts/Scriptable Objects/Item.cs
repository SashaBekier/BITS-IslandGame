
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{

    public Sprite sprite;

    public int maxStack = 10;

    public bool isEquipable = false;
    public int xpReward = 0;

     public virtual void RightClick()
    {
        Debug.Log("Calling Item Right Click");
        
    }
 

    void Start()
    {
        
    }


}
