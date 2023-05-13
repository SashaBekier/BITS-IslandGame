using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/Enemy")]
public class Enemy : ScriptableObject
{

    public Enemy enemy;
    //Using as a data containter
    public string enemyName = "default";
    public int startingHP = 100; // TODO this is a placeholder, put player stats class here instead
    public int test;

    // TODO: create scriptable objects and arrays of them for different moves, e.g. combat.
    //Stats and all that fun stuff. 
    //public Moves[] characterMoves; 

    public Sprite sprite;

    

    public virtual void RightClick()
    {
        Debug.Log("Calling Enemy Right Click");

    }


}

