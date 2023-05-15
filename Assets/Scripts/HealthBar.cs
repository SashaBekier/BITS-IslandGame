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

    void Update()
    {
        currentHealth = warrior.GetComponent<PlayerStats>().currentHealth; // Get the current health from the PlayerStats script attached to the Warrior object
        healthBar.fillAmount = currentHealth / maxHealth; // Set the fill amount of the health bar image based on the current and maximum health values

        // Move the health bar to the left based on the fill amount
        healthBarTransform.offsetMax = new Vector2(-(1 - healthBar.fillAmount) * healthBarTransform.rect.width, healthBarTransform.offsetMax.y);
    }
}