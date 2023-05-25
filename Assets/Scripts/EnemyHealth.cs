using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    private void Start()
    {
        currentHealth = maxHealth; // Set the initial health to the maximum health
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduce the current health by the damage amount

        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if the enemy's health reaches or goes below 0
        }
    }

    private void Die()
    {
        // Implement what should happen when the enemy dies
        // This can include playing death animations, disabling the enemy GameObject, or any other desired behavior
        Debug.Log("Enemy has died!");
        gameObject.SetActive(false); // Disable the enemy GameObject

        // Load the exploration scene
        SceneManager.UnloadSceneAsync("BattleScene");
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // Return the current health of the enemy
    }
}