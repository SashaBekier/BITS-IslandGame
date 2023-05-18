using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject player; // Reference to the Warrior object
    public Image healthBar; // Reference to the health bar image
    public float widthAtFull;
    //public RectTransform healthBarTransform; // Reference to the RectTransform component attached to the health bar image
    private float maxHealth; // The maximum health of the Warrior
    private float lastKnownHealth; // The current health of the Warrior
    private int lastKnownHeroLevel;
    [HideInInspector] public PlayerStats warrior;


 
  
    void Start()
    {
        warrior = player.GetComponent<PlayerStats>();
        maxHealth = warrior.HealthTotal; // Get the maximum health from the PlayerStats script attached to the Warrior object
        lastKnownHealth = warrior.HealthTotal;
        lastKnownHeroLevel = warrior.Level;

        UpdateHealthBar();
    }

    private void Update()
    {
        bool isRefreshNeeded = false;
        if (lastKnownHeroLevel != warrior.Level)
        {
            maxHealth = warrior.HealthTotal;
            isRefreshNeeded = true;
        }
        if (System.Math.Abs(lastKnownHealth-warrior.currentHealth)>0.5) {
            
            isRefreshNeeded = true;
        }
        if(lastKnownHealth > maxHealth)
        {
            maxHealth = warrior.HealthTotal;
            lastKnownHealth = Mathf.Min(maxHealth, lastKnownHealth);
        }
        if (isRefreshNeeded)
        {
            UpdateHealthBar();
            isRefreshNeeded=false;
        }
    }


    // Call this method to update the health bar
    public void UpdateHealthBar()
    {
        if(lastKnownHealth > warrior.currentHealth)
        {
            lastKnownHealth -= 1 / maxHealth;
        } else
        {
            lastKnownHealth += 1 / maxHealth;
        }
        float newWidth = lastKnownHealth / maxHealth * widthAtFull;
        healthBar.rectTransform.sizeDelta = new Vector2(newWidth, 30);
       
    }
}