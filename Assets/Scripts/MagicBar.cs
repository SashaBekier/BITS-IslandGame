using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public GameObject warrior; // Reference to the Warrior object
    public Image magicBar; // Reference to the health bar image
    public RectTransform magicBarTransform; // Reference to the RectTransform component attached to the health bar image

    private float maxMagic; // The maximum health of the Warrior
    private float currentMagic; // The current health of the Warrior

    void Start()
    {
        maxMagic = warrior.GetComponent<PlayerStats>().MagicTotal; // Get the maximum health from the PlayerStats script attached to the Warrior object
    }

    void Update()
    {
        currentMagic = warrior.GetComponent<PlayerStats>().currentMagic; // Get the current health from the PlayerStats script attached to the Warrior object
        magicBar.fillAmount = currentMagic / maxMagic; // Set the fill amount of the health bar image based on the current and maximum health values

        // Move the health bar to the left based on the fill amount
        magicBarTransform.offsetMax = new Vector2(-(1 - magicBar.fillAmount) * magicBarTransform.rect.width, magicBarTransform.offsetMax.y);
    }
}