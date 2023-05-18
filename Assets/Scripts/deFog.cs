using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class deFog : MonoBehaviour
{
    public Tilemap fogTilemap;
    private int vision = 3;
    public Image nightOverlay;
    private Vector3Int lastGridCell;
    // Start is called before the first frame update
    void Start()
    {
        lastGridCell = fogTilemap.WorldToCell(transform.position);
        UpdateFog();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lastGridCell.Equals(fogTilemap.WorldToCell(transform.position)))
        {
            lastGridCell = fogTilemap.WorldToCell(transform.position);
            UpdateFog();
        }
       
    }

    private void UpdateFog()
    {
        int localVision = vision;
        Vector3Int currentPlayerPosition = fogTilemap.WorldToCell(transform.position);
        //Debug.Log(nightOverlay.color.a);
        if (nightOverlay.color.a > 0.2f)
        {
            localVision = vision - 1;
        } else
        {
            localVision = vision;
        }

        for (int i = -localVision; i <= localVision; i++)
        {
            for (int j = -localVision; j <= localVision; j++)
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

