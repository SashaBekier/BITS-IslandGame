using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Fightable : MonoBehaviour
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
            if (battleManager != null)
            {
                battleManager.InitiateBattle(enemy); // Pass the current enemy to the BattleManager
            }
        }
    }

    public virtual void RightClick()
    {
        Debug.Log("Calling Enemy Right Click");
    }
}
