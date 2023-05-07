using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Strength;
    public int Intelligence;
    public int Dexterity;

    [HideInInspector]
    public List<Modifier> modifiers = new List<Modifier>();

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

    //testing
    private void Start()
    {
        Modifier myModifier = new Modifier("Attack", 20);
        modifiers.Add(myModifier);
        Debug.Log("Total Attack: " + AttackTotal);
    }

    public class Modifier
    {
        public int magnitude;
        public string modType;

        public Modifier(string modType, int magnitude)
        {
            this.modType = modType;
            this.magnitude = magnitude;
        }
    }
}