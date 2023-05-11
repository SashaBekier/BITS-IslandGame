using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Edible", menuName = "ScriptableObject/Item - Edible")]
public class Item_Edible : Item
{
    public int healthChange;
    public int magicChange;
    

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
        player.adjustCurrentHealth(healthChange);
        player.adjustCurrentMagic(magicChange);

        Item thisItem = this;
        InventoryManager.instance.DestroyItem(thisItem);
    }

}
