using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    // First spawn player prefab
    public GameObject player;

    // Review: Player Position may already be in other location  

    public Character[] characters;

    public GameObject characterSelectPanel;
    public GameObject playerSpecificStats;

    public Character selectedCharacter;

    public void StartGame(int characterChoice)
    {
        characterSelectPanel.SetActive(false);

        //TODO activate stats panel.
        //playerSpecificStats.SetActive(true);

        selectedCharacter = characters[characterChoice]; 


        GameObject thePlayer = GameObject.Find("Warrior");
        PlayerMouseMovement playerScript = thePlayer.GetComponent<PlayerMouseMovement>();
        playerScript.heroType = characterChoice;
        playerScript.setHeroType = true;

    }

}
