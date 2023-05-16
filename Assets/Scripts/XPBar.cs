using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public GameObject player; // Reference to the Warrior object
    public Image XPBarthing; // Reference to the XP bar image
    public float widthAtFull;
    //public RectTransform XPBarTransform; // Reference to the RectTransform component attached to the XP bar image
    private int lastKnownHeroLevel;
    private int lastKnownXP;
    private int lastKnownTotalXP;
    [HideInInspector] public PlayerStats warrior;


    void Start()
    {
        warrior = player.GetComponent<PlayerStats>();
        lastKnownHeroLevel = warrior.Level;
        lastKnownXP = warrior.currentXP;
        lastKnownTotalXP = warrior.totalXP;

        UpdateXPBar();
    }

    private void Update()
    {
        bool isRefreshNeeded = false;
        if (lastKnownHeroLevel != warrior.Level)
        {
            lastKnownHeroLevel = warrior.Level;
            isRefreshNeeded = true;
        }
        if (lastKnownXP != warrior.currentXP || lastKnownTotalXP != warrior.totalXP)
        {
            lastKnownXP = warrior.currentXP;
            lastKnownTotalXP = warrior.totalXP;
            isRefreshNeeded = true;
        }

        if (isRefreshNeeded)
        {
            UpdateXPBar();
            isRefreshNeeded = false;
        }
    }

    // Call this method to update the XP bar
    public void UpdateXPBar()
    {
        float xpRatio = (float)lastKnownXP / (float)lastKnownTotalXP;
        float newWidth = xpRatio * widthAtFull;
        XPBarthing.rectTransform.sizeDelta = new Vector2(newWidth, 30);
    }
}
