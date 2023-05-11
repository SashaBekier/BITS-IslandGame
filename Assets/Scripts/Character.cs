using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObject/Character")]
public class Character : ScriptableObject
{

    //Using as a data containter
    public string characterName = "default";
    public int startingHP = 100; // TODO this is a placeholder, put player stats class here instead
    public int test;

   // TODO: create scriptable objects and arrays of them for different moves, e.g. combat.
   //Stats and all that fun stuff. 
   //public Moves[] characterMoves; 

}
