using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class BattleManager : MonoBehaviour
{
    public void InitiateBattle(Enemy enemy)
    {
        Debug.Log("Battle initiated with enemy: " + enemy.enemyName);

        // TODO: Implement the battle logic
        // This method is called when a battle is initiated with the specified enemy.
        // You can handle things like spawning enemies, setting up battle UI, transitioning to the battle scene, etc.
        // You can access the enemy's data through the 'enemy' parameter.

        // Example: Transition to a battle scene
        // SceneManager.LoadScene("BattleScene");
        Debug.Log("Battle initiated! Enemy: " + enemy.enemyName);
    }

    // TODO: Add more battle-related methods and logic as needed
}