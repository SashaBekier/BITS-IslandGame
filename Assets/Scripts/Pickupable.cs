using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;

    public SpriteRenderer spriteRenderer;



    public void Start()
    {
        spriteRenderer.sprite = item.image;   
    }

    public void Initialise(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.image;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InventoryManager.instance.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
