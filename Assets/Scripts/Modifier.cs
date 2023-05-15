using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Modifier", menuName = "ScriptableObject/Modifier")]
public class Modifier : ScriptableObject
{
    
        public int magnitude;
        public string modType;

        public void initialise(string modType, int magnitude)
        {
            this.modType = modType;
            this.magnitude = magnitude;
        }
    
}
