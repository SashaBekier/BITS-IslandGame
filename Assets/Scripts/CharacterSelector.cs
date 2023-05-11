using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // First spawn player prefab
    public GameObject player;

    //Review: Player Position may already be in other location
   // public Vector3 playerSpawnPosition = new Vector3 (0, 1, -7);

    public Character[] characters;

    public GameObject characterSelectPanel;
    public GameObject playerSpecificStats;

    public void StartGame(int characterChoice)
    {
        characterSelectPanel.SetActive(false);

        //TODO activate stats panel.
        playerSpecificStats.SetActive(true);

        Character selectedCharacter = characters[characterChoice]; 

    }

}
