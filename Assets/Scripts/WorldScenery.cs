using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class WorldScenery : MonoBehaviour, IPointerClickHandler

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click caught");
            scenery.RightClick();
        }
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click caught");
            PlayerMouseMovement player = GameObject.Find("Warrior").GetComponent<PlayerMouseMovement>();
            player.enqueuePathToMousePosition(false);
        }
    }
}
