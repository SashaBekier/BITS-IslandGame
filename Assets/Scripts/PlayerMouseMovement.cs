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
    private bool isPointerOverGameObject = true;

    public int heroType = 0;
    private bool catchingMoveData = false;


    public AnimatorOverrideController warriorAnimator; //Player Selection animation.
    public AnimatorOverrideController huntressAnimator; //Player Selection animation.

    private Animator animator;
    private Vector3 shim = new Vector3(0f, .0f, 0f);
    private Vector3 previousPosition; //Used for Walking Animations.

    private Vector3 lastCheckpointPosition;
    private Vector3 nextCheckpointPosition;

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
        mouseInput.Mouse.MouseClick.performed += _ => enqueuePathToMousePosition(true);
        targetLocation = transform.position;
        previousPosition= transform.position;
        lastCheckpointPosition = ground.CellToWorld(ground.WorldToCell(targetLocation));

    }

    // Update is called once per frame
    void Update()
    {
        if(catchingMoveData)
        {
            if (mouseInput.Mouse.MouseClick.IsPressed())
            {
                enqueuePathToMousePosition(true);
            } else {
                catchingMoveData=false;
            }
        }
        Vector3 plannedMove = Vector3.MoveTowards(transform.position - shim, targetLocation -shim, moveSpeed * Time.deltaTime) ;
        Vector3Int gridPosition = ground.WorldToCell(plannedMove);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!impassable.HasTile(gridPosition))

        {
            if(Vector3.Distance(transform.position, targetLocation) > 0.1f)
            {
                // Walking animation to use pathing position.
                animator.SetBool("isWalking", true);
                animator.SetFloat("XInput", (transform.position.x-previousPosition.x));
                animator.SetFloat("YInput", (transform.position.y-previousPosition.y));
                previousPosition = transform.position;

                
                
                transform.position = plannedMove + shim;

            } else {
                if (checkpoints.Count > 0)
                {
                    lastCheckpointPosition = ground.CellToWorld(ground.WorldToCell(targetLocation));
                    targetLocation = checkpoints.Dequeue();
                    nextCheckpointPosition = ground.CellToWorld(ground.WorldToCell(targetLocation));
                    
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isPointerOverGameObject = true;
        } else
        {
            isPointerOverGameObject = false;
        }


        if (heroType == 0)
        {
            initialiseWarrior();
        } else if (heroType == 1) 
        {
            initialiseHuntress();
        }

    }

    public void enqueuePathToMousePosition(bool checkPointer)
    {
        if(isPointerOverGameObject && checkPointer)
        {
           return;
        }
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = ground.WorldToCell(mousePosition);

        if (ground.HasTile(gridPosition)&&!impassable.HasTile(gridPosition) )
        {
            Vector3Int playerCell = ground.WorldToCell(transform.position - shim);
            Queue<Vector3Int> pathFound = PathFinder.instance.FindPath(playerCell, gridPosition);
            checkpoints.Clear();
            lastCheckpointPosition = ground.CellToWorld(playerCell);
            
            while (pathFound.Count > 0)
            {
                Vector3Int nextCoords = pathFound.Dequeue();
                Debug.Log("Queuing: (" + nextCoords.x + "," + nextCoords.y + ")");
                //checkpoints.Enqueue(offsetPositionWithinCell(nextCoords,shim.y));
                checkpoints.Enqueue(impassable.CellToWorld(nextCoords)+shim);
            }
        }
        catchingMoveData = true;

    }

    public Vector3 offsetPositionWithinCell(Vector3Int cellPosition, float yOffset)
    {
        Vector3 position = impassable.CellToWorld(cellPosition);
        position += new Vector3(UnityEngine.Random.Range(-.25f, +.25f), UnityEngine.Random.Range(-.2f + yOffset, +.2f + yOffset), 0);
        return position;
    }

     private void initialiseWarrior()
    {
        heroType = 0;
        GetComponent<Animator>().runtimeAnimatorController = warriorAnimator as RuntimeAnimatorController;
    }

    private void initialiseHuntress()
    {
        heroType = 1;
        GetComponent<Animator>().runtimeAnimatorController = huntressAnimator as RuntimeAnimatorController;
    }
}
