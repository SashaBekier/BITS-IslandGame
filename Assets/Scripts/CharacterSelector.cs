using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    // First spawn player prefab
    public GameObject player;

    //Review: Player Position may already be in other location
    public Vector3 playerSpawnPosition = new Vector3 (0, 1, -7);

    public GameObject[] characters;

    public GameObject characterSelectPanel;


    //TODO: use this for adding player stats.
    public GameObject playerSpecificStats;



    // After Character Select
    public void StartGame(int characterChoice)
    {
        characterSelectPanel.SetActive(false);
        //TODO atctivate stats panel? or not?
        //playerSpecificStats.SetActive(true);

        GameObject spawnedPlayer = Instantiate(characters[characterChoice], playerSpawnPosition, Quaternion.identity) as GameObject;

       
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
