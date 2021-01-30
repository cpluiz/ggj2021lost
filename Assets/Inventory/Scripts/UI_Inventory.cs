using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public GameObject uiItemPrefab;
    public GameObject playerHoldingUI;


    List<GameObject> itemsUi;
    Action<Item> CatchItem;
    Func<ItemList, bool> CombineItems;

    public void SetCombineItems(Func<ItemList, bool> func)
    {
        CombineItems = func;
        playerHoldingUI.GetComponent<UI_Item>().SetCombineItems(CombineItems);
    }

    public void SetCatchItemFunc(Action<Item> func)
    { 
        CatchItem = func; 
        playerHoldingUI.GetComponent<UI_Item>().SetCatchItemFunc(CatchItem);
    }

    public void IncludeItem(Item item)
    {
        GameObject newUiItem;
        newUiItem = (GameObject)Instantiate(uiItemPrefab, transform);
        newUiItem.GetComponent<UI_Item>().SetItem(item);
        newUiItem.GetComponent<UI_Item>().SetCatchItemFunc(CatchItem);
        newUiItem.GetComponent<UI_Item>().SetCombineItems(CombineItems);

        itemsUi.Add(newUiItem);
    }

    public void ExcludeItem(Item oldItem)
    {
        Debug.Log("excluding");
        int index = itemsUi.FindIndex(itemUI => itemUI.GetComponent<UI_Item>().GetItem().itemName == oldItem.itemName);
        if (index != -1)
        {
            Destroy(itemsUi[index].gameObject);
            itemsUi.RemoveAt(index);
        }
    }

    public void ReleaseItem(Item oldItem)
    {
        if(playerHoldingUI.GetComponent<UI_Item>().GetItem().itemName == oldItem.itemName)
        {
            playerHoldingUI.GetComponent<UI_Item>().DeactivateItem();
            IncludeItem(oldItem);
        }
    }

    public void DeactivateHeldItem()
    {
        playerHoldingUI.GetComponent<UI_Item>().DeactivateItem();
    }

    public void OnCatchItem(Item newItem, Item oldItem = null)
    {
        int index = itemsUi.FindIndex(itemUI => itemUI.GetComponent<UI_Item>().GetItem().itemName == newItem.itemName);
        if (index != -1)
        {
            playerHoldingUI.GetComponent<UI_Item>().SetItem(newItem);

            Destroy(itemsUi[index].gameObject);
            itemsUi.RemoveAt(index);

            if (oldItem != null)
            {
                IncludeItem(oldItem);
            }
        }
    }


    void Start()
    {
        itemsUi = new List<GameObject>();
    }

    void Update()
    {
    }
}
