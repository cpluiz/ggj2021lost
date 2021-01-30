using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


[System.Serializable]
public class ItemList : ScriptableObject
{
    public List<Item> itemList = new List<Item>();

    [MenuItem("Assets/Create/Inventory Item List")]
    public static ItemList Create()
    {
        ItemList asset = ScriptableObject.CreateInstance<ItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/Inventory/ScriptableObjects/ItemList/ItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static ItemList CreateTempList()
    { return ScriptableObject.CreateInstance<ItemList>(); }


    public static ItemList Create(string name)
    {
        ItemList asset = ScriptableObject.CreateInstance<ItemList>();
        AssetDatabase.CreateAsset(asset, "Assets/Inventory/ScriptableObjects/ItemList/" + name + ".asset");
        AssetDatabase.SaveAssets();

        return asset;
    }
}


[System.Serializable]
public class Receipt : ScriptableObject
{
    public ItemList neededItems;
    public GameObject receiptResultPrefab;

    [MenuItem("Assets/Create/Receipt")]
    public static Receipt Create()
    {
        Receipt asset = ScriptableObject.CreateInstance<Receipt>();

        AssetDatabase.CreateAsset(asset, "Assets/Inventory/ScriptableObjects/Receipts/Receipt.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static Receipt Create(string name)
    {
        Receipt asset = ScriptableObject.CreateInstance<Receipt>();
        AssetDatabase.CreateAsset(asset, "Assets/Inventory/ScriptableObjects/Receipts/" + name + ".asset");
        AssetDatabase.SaveAssets();

        return asset;
    }

}