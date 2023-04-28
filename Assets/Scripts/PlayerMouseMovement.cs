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

    private Animator animator;
    private Vector3 shim = new Vector3(0f, .4f, 0f);

    public Collider2D playerCollider;
    


    private void Awake()
    {
        mouseInput = new MouseInput();
        animator = GetComponent<Animator>();

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
        Vector3 plannedMove = Vector3.MoveTowards(transform.position - shim, targetLocation -shim, moveSpeed * Time.deltaTime) ;
        Vector3Int gridPosition = ground.WorldToCell(plannedMove);

        if (!impassable.HasTile(gridPosition))

        {
            if(Vector3.Distance(transform.position, targetLocation) > 0.1f)
            {
                transform.position = plannedMove + shim;
            }
            
        } else
        {
            gridPosition = ground.WorldToCell(transform.position - shim);
            targetLocation = ground.CellToWorld(gridPosition) + shim;
        }

        // Face the idle animations to the direction of the mouse.
        // TODO: Needs to cancel when moving.
        // TODO: when moving needs the movement animations. 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animator.SetFloat("XInput", (mousePos.x - transform.position.x));
        animator.SetFloat("YInput", (mousePos.y - transform.position.y));
    }

    private void MouseClick()
    {
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = ground.WorldToCell(mousePosition);
        if (ground.HasTile(gridPosition)&&!impassable.HasTile(gridPosition))
        {
            
            targetLocation = ground.CellToWorld(gridPosition) + shim;
            
        }

    }




}
