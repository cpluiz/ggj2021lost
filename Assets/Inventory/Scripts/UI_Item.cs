using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image itemImage;
    Item item;
    Action<Item> CatchItem;
    Func<ItemList, bool> CombineItems;
    PointerEventData pointerEventData;
    GraphicRaycaster raycaster;
    float interval = 0.5f;
    int tap = 0;

    public void SetCatchItem(Action<Item> func)
    { CatchItem = func; }
    public void SetCombineItems(Func<ItemList, bool> func)
    { CombineItems = func; }


    public Image GetItemImage()
    { return itemImage; }

    public void SetItemImage(Sprite newSprite, bool active = true)
    {
        itemImage.sprite = newSprite;
        itemImage.enabled = active;
    }

    public void DeactivateItem()
    {
        item = null;
        itemImage.enabled = false;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        SetItemImage(item.itemIcon);
    }

    public Item GetItem()
    { return item; }

    public void UIItemClicked()
    {
        if (item != null)
            CatchItem(item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var canvas = transform.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            itemImage.transform.SetParent(canvas.transform);
        }

        itemImage.transform.position = Input.mousePosition;
    }

    List<RaycastResult> GetMouseHitting()
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        return results;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.transform.SetParent(transform);

        foreach (RaycastResult result in GetMouseHitting())
        {
            if (result.gameObject.TryGetComponent<UI_Item>(out var uiItem))
            {
                if (uiItem.item != null && this.item != null)
                {
                    ItemList newList = ItemList.CreateTempList();
                    newList.itemList.Add(uiItem.item);
                    newList.itemList.Add(this.item);

                    CombineItems(newList);
                }
                break;
            }
        }

        itemImage.transform.localPosition = Vector3.zero;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        float dragDistance = Vector2.Distance(eventData.pressPosition, eventData.position);
        float dragThreshold = 10f;
        bool isDrag = dragDistance > dragThreshold;

        if (!isDrag && item != null)
        {
            tap++;
            if (tap == 1)
                StartCoroutine(DoubleTapInterval());
            else if (tap > 1)
                tap = 0;
        }
    }

    IEnumerator DoubleTapInterval()
    {
        yield return new WaitForSeconds(interval);

        if (tap == 1)
        {
            UIItemClicked(); //didn't double click
        }
        else
        {
            ItemList newList = ItemList.CreateTempList();
            newList.itemList.Add(item);
            newList.itemList.Add(item);

            CombineItems(newList);
        }

        this.tap = 0;
    }

    public void Start()
    {
        //Set up the new Pointer Event
        pointerEventData = new PointerEventData(GetComponent<EventSystem>());
        //Raycast using the Graphics Raycaster and mouse click position
        raycaster = GetComponentInParent<GraphicRaycaster>();
    }
}
