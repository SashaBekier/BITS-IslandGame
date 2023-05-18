using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Fightable : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update

    public Enemy enemy;

    public SpriteRenderer spriteRenderer;


    public void Start()
    {
        spriteRenderer.sprite = enemy.sprite;
    }

    public void Initialise(Enemy enemy)
    {
        this.enemy = enemy;
        spriteRenderer.sprite = enemy.sprite;
    }

    private BattleManager battleManager;

    // Method to set the BattleManager reference
    public void SetBattleManager(BattleManager manager)
    {
        battleManager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collision with enemy");

            // Check if BattleManager.Instance exists
            if (BattleManager.Instance != null)
            {
                Debug.Log("BattleManager.Instance exists"); // Add this debug log

                // Call the InitiateBattle method of the BattleManager
                BattleManager.Instance.InitiateBattle(this.GetComponent<Fightable>());
            }
            else
            {
                Debug.Log("BattleManager.Instance is null");
            }
        }
    }

    public virtual void RightClick()
    {
        Debug.Log("Calling Enemy Right Click");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click caught");
            PlayerMouseMovement player = GameObject.Find("Warrior").GetComponent<PlayerMouseMovement>();
            player.enqueuePathToMousePosition(false);
        }
    }
}
