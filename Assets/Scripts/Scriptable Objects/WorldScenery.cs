using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScenery : MonoBehaviour
{
    public Scenery scenery;

    public SpriteRenderer spriteRenderer;


    public void Start()
    {
        spriteRenderer.sprite = scenery.sprite;
    }

    public void Initialise(Scenery scenery)
    {
        this.scenery = scenery;
        spriteRenderer.sprite = scenery.sprite;
    }
}
