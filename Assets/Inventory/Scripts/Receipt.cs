using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

