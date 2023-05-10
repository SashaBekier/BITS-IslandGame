using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{

    public Sprite image;

    public int maxStack = 10;

     public virtual void RightClick()
    {
        Debug.Log("Calling Item Right Click");
        
    }
 

    void Start()
    {
        
    }


}
