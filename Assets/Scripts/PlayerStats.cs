using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Strength;
    public int Intelligence;
    public int Dexterity;
    [HideInInspector]
    public int Level;
    public int currentXP;
    public int nextLevelXP;
    public int lastLevelXP;




    [HideInInspector]
    public List<Modifier> modifiers = new List<Modifier>();
    //[HideInInspector]
    public int currentHealth;

    //[HideInInspector]
    public int currentMagic;
   
    public void testUseMagic(int amount)
    {

        adjustCurrentMagic(amount);

    }

    private int FetchModifiers(string modifierType)
    {
        int totalModifier = 0;

        foreach (Modifier mod in modifiers)
        {
            if (mod.modType.Equals(modifierType))
            {
                totalModifier += mod.magnitude;
            }
        }
        return totalModifier;
    }

    public void adjustCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > HealthTotal) {
        currentHealth = HealthTotal;
        }
        if (currentHealth < 1)
        {
            playerDeath();
        }
        if (amount > 0)
        {
            Debug.Log(amount + "Health Gained");
        } else
        {
            Debug.Log(amount*-1 + "Health Lost");
        }
        
    }


    private void playerDeath()
    {
        Debug.Log("Player has died");
    }

    public bool adjustCurrentMagic(int amount)
    {
        currentMagic += amount;
        if (currentMagic > MagicTotal)
        {
            currentMagic = MagicTotal;
        }
        if (currentMagic < 0) {
            currentMagic += amount;
            return false;
        } else
        {
            
            return true;
        }
         
    }
    public void drainCurrentMagic(int amount)
    {
        currentMagic += amount;
        if (currentMagic > MagicTotal)
        {
            currentMagic = MagicTotal;
        }
        if (currentMagic < 0)
        {
            currentMagic = 0;
        }
        if (amount > 0)
        {
            Debug.Log(amount + "Magic Gained");
        }
        else
        {
            Debug.Log(amount * -1 + "Magic Lost");
        }
    }

    public int HealthTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Health");
            total += Strength * 2;
            return total;
        }
    }

    public int StrengthTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Strength");
            total += Strength;
            return total;
        }
    }
    public int IntelligenceTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Intelligence");
            total += Intelligence;
            return total;
        }
    }
    public int DexterityTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Dexterity");
            total += Dexterity;
            return total;
        }
    }
    public int AttackTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Attack");
            total += Strength * 2;
            return total;
        }
    }
    public int MpTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Mp");
            total += Intelligence * 2;
            return total;
        }
    }
    public int MagicTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Magic");
            total += Intelligence * 2;
            return total;
        }
    }
    public int RangedTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Ranged");
            total += Dexterity * 2;
            return total;
        }
    }
    public int MovementTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Movement");
            total += Dexterity * 2;
            return total;
        }
    }
    public int SpeedTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers("Speed");
            total += Dexterity * 2;
            return total;
        }
    }
    public void AdjustXP(int xpAmount)
    {
        currentXP += xpAmount;
        

        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        
        Level++;
        Debug.Log("Level Up! You are now level " + Level);
        lastLevelXP = nextLevelXP;
        nextLevelXP += 900 + 100 * Level;
        Intelligence += 1;
        int heroType = GameObject.Find("Warrior").GetComponent<PlayerMouseMovement>().heroType;
        if (heroType == 0 ) {
            Strength += 3;
            Dexterity += 2;
        } else if(heroType == 1 )
        {
            Strength += 2;
            Dexterity += 3;
        }
        currentHealth = HealthTotal;
        currentMagic = MagicTotal;
    }

    public int GetCurrentXP()
    {
        return currentXP;
    }

    public int GetTotalXP()
    {
        return nextLevelXP;
    }




    //testing
    private void Start()
    {
        currentHealth = HealthTotal;
        currentMagic = MagicTotal;
        nextLevelXP = 900 * Level;
        for(int i = 1; i < Level+1; i++)
        {
            nextLevelXP += Level * 100;
            
        }
        lastLevelXP = nextLevelXP - 900 - (100 * Level);
        Modifier myModifier = (Modifier)ScriptableObject.CreateInstance("Modifier");
        myModifier.initialise("Strength", 20);
        modifiers.Add(myModifier);
        Debug.Log("Total Attack: " + AttackTotal);
    }


}
