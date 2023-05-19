using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public int Strength;
    public int Intelligence;
    public int Dexterity;
    [HideInInspector]
    public int Level;
    public int currentXP;
    public int nextLevelXP;
    public int lastLevelXP;
    
    public enum Modifiable
    {
        Strength,
        Intelligence, 
        Dexterity,
        Health,
        Magic, 
        Ranged,
        Speed,
        Attack,
        Defence
    }
    private void Awake()
    {
        instance = this;
    }

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

    private int FetchModifiers(Modifiable modifierType)
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
            total += FetchModifiers(Modifiable.Health);
            total += Strength * 2;
            return total;
        }
    }

    public int StrengthTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Strength);
            total += Strength;
            return total;
        }
    }
    public int IntelligenceTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Intelligence);
            total += Intelligence;
            return total;
        }
    }
    public int DexterityTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Dexterity);
            total += Dexterity;
            return total;
        }
    }
    public int AttackTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Attack);
            total += StrengthTotal * 2;
            return total;
        }
    }
    public int DefenceTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Defence);
            total += DexterityTotal * 2;
            return total;
        }
    }
    public int MpTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Magic);
            total += Intelligence * 2;
            return total;
        }
    }
    public int MagicTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Magic);
            total += IntelligenceTotal * 2;
            return total;
        }
    }
    public int RangedTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Ranged);
            total += DexterityTotal * 2;
            return total;
        }
    }
    public int MovementTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Speed);
            total += Dexterity * 2;
            return total;
        }
    }
    public int SpeedTotal
    {
        get
        {
            int total = 0;
            total += FetchModifiers(Modifiable.Speed);
            total += DexterityTotal * 2;
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
        myModifier.initialise(Modifiable.Strength, 20);
        modifiers.Add(myModifier);
        Debug.Log("Total Attack: " + AttackTotal);
    }


}
