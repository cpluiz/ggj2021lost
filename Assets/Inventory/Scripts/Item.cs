using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]                         //    Our Representation of an InventoryItem
public class Item : MonoBehaviour
{
    public string itemName = "New Item";      //    What the item will be called in the inventory
    public Sprite itemIcon = null;         //    What the item will look like in the inventory
    public Rigidbody2D itemObject = null;       //    Optional slot for a PreFab to instantiate when discarding
    public bool isUnique = false;             //    Optional checkbox to indicate that there should only be one of these items per game
    public bool isQuestItem = false;          //    Examples of additional information that could be held in InventoryItem
    public bool isStackable = false;          //    Examples of additional information that could be held in InventoryItem
    public bool destroyOnUse = false;         //    Examples of additional information that could be held in InventoryItem
    public float encumbranceValue = 0;        //    Examples of additional information that could be held in InventoryItem  !!!


    public void CopyItem(Item item)
    {
        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemObject = item.itemObject;
        isUnique = false;
        isQuestItem = item.isQuestItem;
        isStackable = item.isStackable;
        destroyOnUse = item.destroyOnUse;
        encumbranceValue = item.encumbranceValue;
    }
}