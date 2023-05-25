using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitiateBattle(Fightable fightable)
    {
        Debug.Log("Battle initiated with enemy: " + fightable.enemy.enemyName);

        // Save player's position
        SavePlayerPosition();

        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
    }

    private void SavePlayerPosition()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            // Save the player position to your save data or perform any other necessary actions
        }
    }
}