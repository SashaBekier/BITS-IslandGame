using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    
    [Header("UI")]
    public Image image;
    public Text countText;
    public Text dropText;

    

    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Item item;



    // Start is called before the first frame update
    void Start()
    {
 
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void DestroyItem()
    {
        Destroy(this);
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        dropText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
        dropText.gameObject.SetActive(textActive);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("Right Click caught");
            item.RightClick();
        }
    }

}
