using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Item_Equipable", menuName = "ScriptableObject/Equipable Item")]
public class Item_Equipable : Item
{
    public EquipManager.EquipSlotNames equipSlotRequired;
    [HideInInspector]
    public EquipSlot equipSlot;
    public Modifier[] modifiers;
    public bool is2Handed = false;
   

    public void OnAwake()
    {
        equipSlot = (EquipSlot) EquipManager.equipManager.inventorySlots[(int)equipSlotRequired];
        
    }


}
