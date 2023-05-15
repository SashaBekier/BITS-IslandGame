using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : InventoryManager
{
    public static EquipManager equipManager;
    private PlayerStats player;
    public GameObject equipItemPrefab;
    public enum EquipSlotNames
    {
        Head,
        Neck,
        Torso,
        OnHand,
        OffHand,
        Waist,
        Legs,
        Feet,
        Charm
    }

    private void Awake()
    {
        equipManager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Warrior").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool don(Item_Equipable itemToDon)
    {
        Debug.Log("Donning " + itemToDon + "Player Strength = " + player.StrengthTotal);
        int slotIndex = (int)itemToDon.equipSlotRequired;
        if (slotIndex == (int)EquipSlotNames.OffHand) //This catches equiping to off hand with a 2 handed item in onHand
        {
            Debug.Log("Offhand Donning detected");
            EquipItem onHand = inventorySlots[(int)EquipSlotNames.OnHand].GetComponentInChildren<EquipItem>();
            if (onHand == null)
            {
                Debug.Log("On hand empty");
            } else if (onHand.equipableItem.is2Handed)
            {
                Debug.Log("On hand Equip is 2-Handed");
                doff((int)EquipSlotNames.OnHand);
            }
        }
        if (doff(slotIndex)&&(!itemToDon.is2Handed || doff((int)EquipSlotNames.OffHand))) // This catches equipping to onHand with a 2-H item
        {
            PutEquipmentAtSlot(itemToDon, (EquipSlot)inventorySlots[slotIndex]);
            foreach (Modifier modifier in itemToDon.modifiers)
            {
                player.modifiers.Add(modifier);
                Debug.Log("Donned " + itemToDon + "Player Strength = " + player.StrengthTotal);
            }
            return true;
        } else
        {
            return false;
        }
    }

    public void PutEquipmentAtSlot(Item_Equipable item, EquipSlot slot)
    {
        GameObject newItem = Instantiate(equipItemPrefab, slot.transform);
        EquipItem equipItem = newItem.GetComponent<EquipItem>();
        Debug.Log("Item about to get sent to initialise " + item + " with image " + item.sprite);
        equipItem.InitialiseItem(item);
    }

    public bool doff(int slotIndex)
    {
        Debug.Log("Doffing slot " + (EquipSlotNames)slotIndex + "Player Strength = " + player.StrengthTotal);
        bool slotClear = false;
        EquipItem itemToDoff = inventorySlots[slotIndex].GetComponentInChildren<EquipItem>();
        if (itemToDoff == null)
        {
            slotClear = true;
        } else
        {
            if (InventoryManager.instance.AddItem(itemToDoff.item))
            {
                foreach (Modifier modifier in (itemToDoff.equipableItem.modifiers))
                {
                    player.modifiers.Remove(modifier);
                }
                Debug.Log("Doffed " + itemToDoff + "Player Strength = " + player.StrengthTotal);
                Destroy(itemToDoff.gameObject);
                slotClear = true;
            }
            
        }
        return slotClear;
    }
}


