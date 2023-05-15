using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

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
        image.sprite = item.sprite;
        
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
