using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public GameObject uiItemPrefab;
    public GameObject rightHolderUI;
    public GameObject leftHolderUI;


    List<GameObject> itemsUi;
    Action<Item> CatchItem;
    Func<ItemList, bool> CombineItems;

    public void CombineHeldItems()
    {
        var rightItem = rightHolderUI.GetComponent<UI_Item>().GetItem();
        var leftItem = leftHolderUI.GetComponent<UI_Item>().GetItem();

        if (rightItem == null && leftItem == null)
        {
            return;
        }
        else if (rightItem != null && leftItem != null)
        {
            ItemList newList = ItemList.CreateTempList();
            newList.itemList.Add(rightItem);
            newList.itemList.Add(leftItem);
            CombineItems(newList);
        }
        else if (rightItem != null)
        {
            ItemList newList = ItemList.CreateTempList();
            newList.itemList.Add(rightItem);
            newList.itemList.Add(rightItem);
            CombineItems(newList);
        }
        else
        {
            ItemList newList = ItemList.CreateTempList();
            newList.itemList.Add(leftItem);
            newList.itemList.Add(leftItem);
            CombineItems(newList);
        }
    }

    public void SetCombineItems(Func<ItemList, bool> func)
    {
        CombineItems = func;
        rightHolderUI.GetComponent<UI_Item>().SetCombineItems(CombineItems);
        leftHolderUI.GetComponent<UI_Item>().SetCombineItems(CombineItems);
    }

    public void SetCatchItemFunc(Action<Item> func)
    {
        CatchItem = func;
        rightHolderUI.GetComponent<UI_Item>().SetCatchItem(CatchItem);
        leftHolderUI.GetComponent<UI_Item>().SetCatchItem(CatchItem);
    }

    public void IncludeItem(Item item)
    {
        if (uiItemPrefab == null) return;

        GameObject newUiItem;
        newUiItem = (GameObject)Instantiate(uiItemPrefab, transform);
        newUiItem.GetComponent<UI_Item>().SetItem(item);
        newUiItem.GetComponent<UI_Item>().SetCatchItem(CatchItem);
        newUiItem.GetComponent<UI_Item>().SetCombineItems(CombineItems);
    
        itemsUi.Add(newUiItem);
    }

    public void ExcludeItem(Item oldItem)
    {
        int index = itemsUi.FindIndex(itemUI => itemUI.GetComponent<UI_Item>().GetItem().itemName == oldItem.itemName);
        if (index != -1)
        {
            Destroy(itemsUi[index].gameObject);
            itemsUi.RemoveAt(index);
        }
    }

    public void ReleaseItem(Item oldItem, bool rightHand)
    {
        if (rightHand)
        {
            if (rightHolderUI.GetComponent<UI_Item>().GetItem().itemName == oldItem.itemName)
            {
                rightHolderUI.GetComponent<UI_Item>().DeactivateItem();
            }
        }
        else
        {
            if (leftHolderUI.GetComponent<UI_Item>().GetItem().itemName == oldItem.itemName)
            {
                leftHolderUI.GetComponent<UI_Item>().DeactivateItem();
            }
        }
    }

    public void DeactivateHeldItem(bool rightHand)
    {
        if (rightHand)
            rightHolderUI.GetComponent<UI_Item>().DeactivateItem();
        else
            leftHolderUI.GetComponent<UI_Item>().DeactivateItem();
    }

    public void OnCatchItem(Item newItem, bool rightHand)
    {
        if (rightHand)
            rightHolderUI.GetComponent<UI_Item>().SetItem(newItem);
        else
            leftHolderUI.GetComponent<UI_Item>().SetItem(newItem);
    }


    void Start()
    {
        itemsUi = new List<GameObject>();
    }

    void Update()
    {
    }
}
