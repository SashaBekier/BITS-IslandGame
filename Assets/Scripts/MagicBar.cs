using UnityEngine;
using UnityEngine.UI;

public class MagicBar : MonoBehaviour
{
    public GameObject player; // Reference to the Warrior object
    public Image magicBar; // Reference to the health bar image
    public float widthAtFull;
    //public RectTransform healthBarTransform; // Reference to the RectTransform component attached to the health bar image
    private float maxMagic; // The maximum health of the Warrior
    private float lastKnownMagic; // The current health of the Warrior
    private int lastKnownHeroLevel;
    [HideInInspector] public PlayerStats warrior;




    void Start()
    {
        warrior = player.GetComponent<PlayerStats>();
        maxMagic = warrior.MagicTotal; // Get the maximum health from the PlayerStats script attached to the Warrior object
        lastKnownMagic = warrior.MagicTotal;
        lastKnownHeroLevel = warrior.Level;

        UpdateMagicBar();
    }

    private void Update()
    {
        bool isRefreshNeeded = false;
        if (lastKnownHeroLevel != warrior.Level)
        {
            maxMagic = warrior.MagicTotal;
            isRefreshNeeded = true;
        }
        if (System.Math.Abs(lastKnownMagic - warrior.currentMagic) > 0.5)
        {

            isRefreshNeeded = true;
        }
        if(lastKnownMagic > maxMagic)
        {
            maxMagic = warrior.MagicTotal;
            lastKnownMagic = Mathf.Min(maxMagic, lastKnownMagic);
        }

        if (isRefreshNeeded)
        {
            UpdateMagicBar();
            isRefreshNeeded = false;
        }
    }


    // Call this method to update the health bar
    public void UpdateMagicBar()
    {
        if (lastKnownMagic > warrior.currentMagic)
        {
            lastKnownMagic -= 1 / maxMagic;
        }
        else
        {
            lastKnownMagic += 1 / maxMagic;
        }
        float newWidth = (lastKnownMagic / maxMagic) * widthAtFull;
        magicBar.rectTransform.sizeDelta = new Vector2(newWidth, 30);

    }
}