using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (transform.childCount == 0)
        {
            
            inventoryItem.parentAfterDrag = transform;
        } else
        {
            InventoryItem itemInThisSlot = this.GetComponentInChildren<InventoryItem>();
            itemInThisSlot.moveToSlot(inventoryItem.parentAfterDrag);
            inventoryItem.parentAfterDrag = transform;
        }
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
