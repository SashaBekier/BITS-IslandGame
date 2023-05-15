using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : InventorySlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        EquipItem equipItem;
        if (inventoryItem.GetType() != typeof(EquipItem)){
            return;
        } else
        {
            equipItem = (EquipItem) inventoryItem;
        }
        if(equipItem.equipableItem.equipSlot != this)
        {
            return;
        }
        base.OnDrop(eventData);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
