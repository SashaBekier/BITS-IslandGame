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
    private float moveSpeed = 5;

    private Animator animator;

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
    }

    // Update is called once per frame
    void Update()
    { 
        if(Vector3.Distance(transform.position, targetLocation) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, moveSpeed * Time.deltaTime);
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
            targetLocation = ground.CellToWorld(gridPosition);
            targetLocation += new Vector3(0f, .4f, 0f);

        }

    }
}
