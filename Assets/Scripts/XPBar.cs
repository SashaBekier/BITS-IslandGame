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
    private int nextLevelXP;
    [HideInInspector] public PlayerStats warrior;
    private int lastLevelXP;


    void Start()
    {
        warrior = player.GetComponent<PlayerStats>();
        lastKnownHeroLevel = warrior.Level;
        lastKnownXP = warrior.currentXP;
        nextLevelXP = warrior.nextLevelXP;
        lastLevelXP = warrior.lastLevelXP;
        UpdateXPBar();
    }

    private void Update()
    {
        bool isRefreshNeeded = false;
        if (lastKnownHeroLevel != warrior.Level)
        {
            lastKnownHeroLevel = warrior.Level;
            lastLevelXP = nextLevelXP;
            nextLevelXP = warrior.nextLevelXP;
            isRefreshNeeded = true;
        }
        if (lastKnownXP != warrior.currentXP )
        {
            lastKnownXP = warrior.currentXP;
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
        float xpRatio = ((float)lastKnownXP - lastLevelXP) / ((float)nextLevelXP-lastLevelXP);
        float newWidth = xpRatio * widthAtFull;
        XPBarthing.rectTransform.sizeDelta = new Vector2(newWidth, 5);
    }
}
