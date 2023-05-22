using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

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

        SceneManager.LoadScene("BattleScene");
    }

}