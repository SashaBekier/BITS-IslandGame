using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Edible", menuName = "ScriptableObject/Item - Edible")]
public class Item_Edible : Item
{
    public int healthGain;
    public int magicGain;
    

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
        Debug.Log("Item_Edible Right Click Called");
        PlayerStats player = GameObject.Find("Warrior").GetComponent<PlayerStats>();
        player.gainHealth(healthGain);
        player.gainMagic(magicGain);

        Item thisItem = this;
        InventoryManager.instance.DestroyItem(thisItem);
    }

}
