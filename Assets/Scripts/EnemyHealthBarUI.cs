using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Slider slider;

    private EnemyHealth enemyHealth;

    private void Start()
    {
        enemyHealth = GameObject.Find("Enemy").GetComponent<EnemyHealth>();

        // Set the maximum value of the health bar to the enemy's maximum health
        slider.maxValue = enemyHealth.maxHealth;
    }

    private void Update()
    {
        // Update the current value of the health bar to the enemy's current health
        slider.value = enemyHealth.GetCurrentHealth();
    }
}