using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipItem : InventoryItem
{


    public Item_Equipable equipableItem;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        equipableItem = (Item_Equipable)item;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitialiseItem(Item_Equipable newEquip)
    {
        item = newEquip;
        equipableItem = newEquip;
        Debug.Log("Item received at initialise " + item + " with image " + item.sprite);
        Sprite sprite = item.sprite;
        Debug.Log(sprite + " is the setting for sprite");

        //this line demonstrates that for reasons unknown image.sprite doesn't exist here even though
        //it works fine in InventoryItem and this extends InventoryItem without making any adjustments to the image variable
        Debug.Log(image.sprite + "is the image.sprite before intitialise");
        //image.sprite = sprite;
        //RefreshCount();
    }



    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click caught");
            if (InventoryManager.instance.AddItem(item))
                {
                    int slotIndex = (int)equipableItem.equipSlotRequired;
                    EquipManager.equipManager.doff(slotIndex);

                }
                
            
            //item.RightClick();
        }
    }
}
