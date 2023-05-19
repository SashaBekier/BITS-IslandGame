using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObject/Modifier")]
public class Modifier : ScriptableObject
{
    
        public int magnitude;
        public PlayerStats.Modifiable modType;

        public void initialise(PlayerStats.Modifiable modType, int magnitude)
        {
            this.modType = modType;
            this.magnitude = magnitude;
        }

    public static Modifier newModifier(PlayerStats.Modifiable modType, int magnitude)
    {
        Modifier myModifier = (Modifier)ScriptableObject.CreateInstance("Modifier");
        myModifier.initialise(modType, magnitude);
        return myModifier;
    }
    
}
