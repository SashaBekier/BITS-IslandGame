using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMouseMovement : MonoBehaviour
{
    MouseInput mouseInput;
    public Tilemap ground;
    public Tilemap impassable;
    private Vector3 targetLocation;
    private Vector3 lastGoodPos;
    private float moveSpeed = 5;
    public Collider2D playerCollider;
    

    private void Awake()
    {
        mouseInput = new MouseInput();
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }

    private void OnDisable()
    {
        mouseInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        mouseInput.Mouse.MouseClick.performed += _ => MouseClick();
        targetLocation = transform.position;
        lastGoodPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 plannedMove = Vector3.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
        Vector3Int gridPosition = ground.WorldToCell(plannedMove);

        if (!impassable.HasTile(gridPosition))
        {
            if(Vector3.Distance(transform.position, targetLocation) > 0.1f)
            {
                transform.position = plannedMove;
            }
            
        } else
        {
            gridPosition = ground.WorldToCell(transform.position);
            targetLocation = ground.CellToWorld(gridPosition);
        }
        
    }

    private void MouseClick()
    {
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = ground.WorldToCell(mousePosition);
        if (ground.HasTile(gridPosition)&&!impassable.HasTile(gridPosition))
        {
            
            targetLocation = ground.CellToWorld(gridPosition);
            //targetLocation += new Vector3(0f, .4f, 0f);
        }

    }




}
