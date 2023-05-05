using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class deFog : MonoBehaviour
{
    public Tilemap fogTilemap;
    private int vision = 3;
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

        for (int i = -vision; i <= vision; i++)
        {
            for (int j = -vision; j <= vision; j++)
            {
                fogTilemap.SetTile(currentPlayerPosition + new Vector3Int(i, j, 0), null);
            }
        }
    }
}
