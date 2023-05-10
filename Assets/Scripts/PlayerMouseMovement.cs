using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Data;
using System.Linq;

public class PlayerMouseMovement : MonoBehaviour
{
    MouseInput mouseInput;
    public Tilemap ground;
    public Tilemap impassable;
    private Vector3 targetLocation;
    private Queue<Vector3> checkpoints = new Queue<Vector3>();
    private float moveSpeed = 2.5f;
    

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

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 plannedMove = Vector3.MoveTowards(transform.position - shim, targetLocation -shim, moveSpeed * Time.deltaTime) ;
        Vector3Int gridPosition = ground.WorldToCell(plannedMove);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!impassable.HasTile(gridPosition))

        {
            if(Vector3.Distance(transform.position, targetLocation) > 0.1f)
            {
                transform.position = plannedMove + shim;

                //TODO: walking animation to use pathing position, not 'mousePos'.
                animator.SetBool("isWalking", true);
                animator.SetFloat("XInput", (mousePos.x - transform.position.x));
                animator.SetFloat("YInput", (mousePos.y - transform.position.y));
            } else {
                if (checkpoints.Count > 0)
                {
                    targetLocation = checkpoints.Dequeue();
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    animator.SetFloat("XInput", (mousePos.x - transform.position.x));
                    animator.SetFloat("YInput", (mousePos.y - transform.position.y));

                }

            }
            
        } else
        {
            gridPosition = ground.WorldToCell(transform.position - shim);
            targetLocation = ground.CellToWorld(gridPosition) + shim;
        }
        
    }

    private void MouseClick()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = ground.WorldToCell(mousePosition);
        if (ground.HasTile(gridPosition)&&!impassable.HasTile(gridPosition))
        {
            Vector3Int playerCell = ground.WorldToCell(transform.position - shim);
            Queue<Vector3Int> pathFound = PathFinder.instance.FindPath(playerCell, gridPosition);
            checkpoints.Clear();
            
            while (pathFound.Count > 0)
            {
                Vector3Int nextCoords = pathFound.Dequeue();
                Debug.Log("Queuing: (" + nextCoords.x + "," + nextCoords.y + ")");
                checkpoints.Enqueue(ground.CellToWorld(nextCoords) + shim);
            }
        }

    }

    
}
