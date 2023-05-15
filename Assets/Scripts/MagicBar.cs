using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public GameObject warrior; // Reference to the Warrior object
    public Image magicBar; // Reference to the magic bar image
    public RectTransform magicBarTransform; // Reference to the RectTransform component attached to the magic bar image
    private float maxMagic; // The maximum magic of the Warrior
    private float currentMagic; // The current magic of the Warrior

    void Start()
    {
        maxMagic = warrior.GetComponent<PlayerStats>().MagicTotal; // Get the maximum magic from the PlayerStats script attached to the Warrior object
        Debug.Log("Max Magic: " + maxMagic);
    }

    // Call this method to update the magic bar
    public void UpdateMagicBar()
    {
        currentMagic = warrior.GetComponent<PlayerStats>().currentMagic;
        magicBar.fillAmount = currentMagic / maxMagic;

        // Shrink the magic bar from both ends based on the fill amount
        float barWidth = magicBarTransform.rect.width;
        float shrinkAmount = (1 - magicBar.fillAmount) * barWidth / 2;
        magicBarTransform.offsetMax = new Vector2(-shrinkAmount, magicBarTransform.offsetMax.y);
        magicBarTransform.offsetMin = new Vector2(shrinkAmount, magicBarTransform.offsetMin.y);
    }
}