using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsScreen : MonoBehaviour
{
    public Canvas statsCanvas; // Reference to your stats screen canvas

    // Reference to the UI text elements that will display the stats variables
    public Text healthText;
    public Text strengthText;
    public Text intelligenceText;
    public Text dexterityText;
    public Text attackText;
    public Text mpText;
    public Text magicText;
    public Text rangedText;
    public Text movementText;
    public Text speedText;
    public Text xpText;
    public Text levelText;
    public Text totalxpText;

    void Update()
    {
        // Get the player's stats values from the public int variables in the PlayerStats script attached to the "Warrior" object
        GameObject warrior = GameObject.Find("Warrior");
        int healthValue = warrior.GetComponent<PlayerStats>().HealthTotal;
        int strengthValue = warrior.GetComponent<PlayerStats>().StrengthTotal;
        int intelligenceValue = warrior.GetComponent<PlayerStats>().IntelligenceTotal;
        int dexterityValue = warrior.GetComponent<PlayerStats>().DexterityTotal;
        int attackValue = warrior.GetComponent<PlayerStats>().AttackTotal;
        int mpValue = warrior.GetComponent<PlayerStats>().MpTotal;
        int magicValue = warrior.GetComponent<PlayerStats>().MagicTotal;
        int rangedValue = warrior.GetComponent<PlayerStats>().RangedTotal;
        int movementValue = warrior.GetComponent<PlayerStats>().MovementTotal;
        int speedValue = warrior.GetComponent<PlayerStats>().SpeedTotal;
        int xpValue = warrior.GetComponent<PlayerStats>().currentXP;
        int levelValue = warrior.GetComponent<PlayerStats>().Level;
        int totalxpValue = warrior.GetComponent<PlayerStats>().totalXP;

        // Set the text elements to display the player's stats values
        healthText.text = "Health: " + healthValue.ToString();
        strengthText.text = "Strength: " + strengthValue.ToString();
        intelligenceText.text = "Intelligence: " + intelligenceValue.ToString();
        dexterityText.text = "Dexterity: " + dexterityValue.ToString();
        attackText.text = "Attack: " + attackValue.ToString();
        mpText.text = "MP: " + mpValue.ToString();
        magicText.text = "Magic: " + magicValue.ToString();
        rangedText.text = "Ranged: " + rangedValue.ToString();
        movementText.text = "Movement: " + movementValue.ToString();
        speedText.text = "Speed: " + speedValue.ToString();
        xpText.text = "XP: " + xpValue.ToString();
        levelText.text = "Level: " + levelValue.ToString();
        totalxpText.text = "Total XP: " + totalxpValue.ToString();
    }
}
