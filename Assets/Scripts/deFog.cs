using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class deFog : MonoBehaviour
{
    public Tilemap fogTilemap;
    private int vision = 3;
    public Image clock;
    // Start is called before the first frame update
    void Start()
    {
        UpdateFog();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFog();
    }

    private void UpdateFog()
    {
        Vector3Int currentPlayerPosition = fogTilemap.WorldToCell(transform.position);
        if(vision == 3 && clock.transform.eulerAngles.z > 180)
        {
            vision -= 1;
        } else if(vision == 2 && clock.transform.eulerAngles.z > 0)
        {
            vision += 1;
        }

        for (int i = -vision; i <= vision; i++)
        {
            for (int j = -vision; j <= vision; j++)
            {
                Vector3Int testingTile = currentPlayerPosition + new Vector3Int(i, j, 0);
                if (fogTilemap.HasTile(testingTile)) {
                    fogTilemap.SetTile(testingTile, null);
                    GameObject.Find("Warrior").GetComponent<PlayerStats>().AdjustXP(1);
                }
            }
        }
    }
}

