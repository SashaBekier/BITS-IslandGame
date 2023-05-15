using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Fightable : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update

    public Enemy enemy;

    public SpriteRenderer spriteRenderer;


    public void Start()
    {
        spriteRenderer.sprite = enemy.sprite;
    }

    public void Initialise(Enemy enemy)
    {
        this.enemy = enemy;
        spriteRenderer.sprite = enemy.sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click enemy");
            enemy.RightClick();
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click enemy");
            PlayerMouseMovement player = GameObject.Find("Warrior").GetComponent<PlayerMouseMovement>();
            player.enqueuePathToMousePosition(false);
        }
    }
}
