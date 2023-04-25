using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 velocity;
    private Vector3 direction;
    private bool hasMoved;
    public Tilemap fogTilemap;
    private int vision = 3;

    void Start()
    {
        UpdateFog();
    }


    // Update is called once per frame
    void Update()
    {
        if(velocity.x ==0)
        {
            hasMoved = false;    
        } else if(velocity.x!= 0 && !hasMoved)
        {
            hasMoved=true;
            MoveByDirection();
        }
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void MoveByDirection()
    {
        if (velocity.x < 0)
        {
            if(velocity.y>0) 
            {
                direction = new Vector3(-0.5f, 0.5f); 
            } else if (velocity.y < 0)
            {
                direction = new Vector3(-0.5f, -0.5f);
            } else
            {
                direction = new Vector3(-1, 0);
            }
        } else if(velocity.x>0)
        {
            if (velocity.y > 0)
            {
                direction = new Vector3(0.5f, 0.5f);
            }
            else if (velocity.y < 0)
            {
                direction = new Vector3(0.5f, -0.5f);
            }
            else
            {
                direction = new Vector3(1, 0);
            }
        }
        transform.position += direction;
        UpdateFog();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position -= direction;
    }

    private void UpdateFog()
    {
        Vector3Int currentPlayerPosition = fogTilemap.WorldToCell(transform.position);

        for (int i = -vision; i<= vision; i++)
        {
            for (int j = -vision; j<= vision; j++)
            {
                fogTilemap.SetTile(currentPlayerPosition + new Vector3Int(i,j,0), null);
            }
        }
    }
}
