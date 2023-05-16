using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Edible", menuName = "ScriptableObject/Item - Edible")]
public class Item_Edible : Item
{
    public int healthChange;
    public int magicChange;
    public int xpChange;
     
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void RightClick()
    {
        PlayerStats player = GameObject.Find("Warrior").GetComponent<PlayerStats>();
        Debug.Log("Item_Edible Right Click Called");
        if (healthChange != 0) { player.adjustCurrentHealth(healthChange); }
        if (magicChange != 0) { player.drainCurrentMagic(magicChange); }
        if (xpChange != 0) { player.AdjustXP(xpChange); }
        Item thisItem = this;
        InventoryManager.instance.DestroyItem(thisItem);
    }

}
