using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject warrior; // Reference to the Warrior object
    public Image healthBar; // Reference to the health bar image
    public RectTransform healthBarTransform; // Reference to the RectTransform component attached to the health bar image
    private float maxHealth; // The maximum health of the Warrior
    private float currentHealth; // The current health of the Warrior

    void Start()
    {
        maxHealth = warrior.GetComponent<PlayerStats>().HealthTotal; // Get the maximum health from the PlayerStats script attached to the Warrior object
    }


    // Call this method to update the health bar
    public void UpdateHealthBar()
    {
        currentHealth = warrior.GetComponent<PlayerStats>().currentHealth;
        healthBar.fillAmount = currentHealth / maxHealth;

        // Shrink the health bar from both ends based on the fill amount
        float barWidth = healthBarTransform.rect.width;
        float shrinkAmount = (1 - healthBar.fillAmount) * barWidth / 2;
        healthBarTransform.offsetMax = new Vector2(-shrinkAmount, healthBarTransform.offsetMax.y);
        healthBarTransform.offsetMin = new Vector2(shrinkAmount, healthBarTransform.offsetMin.y);
    }
}