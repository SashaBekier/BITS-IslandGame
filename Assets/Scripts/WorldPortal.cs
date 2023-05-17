using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPortal : WorldScenery
{
    public Sprite burnedOutPortal;

    bool isActive = true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            Debug.Log("Portal activated");
            spriteRenderer.sprite = burnedOutPortal;
            GameObject warrior = GameObject.Find("Warrior");

            warrior.GetComponent<PlayerMouseMovement>().ClearCheckpoints();
            if (warrior.transform.position.x < 0)
            {
                warrior.transform.position = PathFinder.instance.impassable.CellToWorld(PuzzleManager.instance.portalLocation);
            }
            else
            {
                warrior.transform.position = PathFinder.instance.impassable.CellToWorld(new Vector3Int(-14, 5, 0));
            }
            warrior.GetComponent<PlayerMouseMovement>().ClearCheckpoints();

            isActive = false;
        } 
    }
}
