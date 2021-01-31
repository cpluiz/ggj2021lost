using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Radio : Item
{
    [SerializeField]
    int batteryPercentage = 0;

    public override bool UniqueCatch()
    {
        base.UniqueCatch();
        ShowHiddenItems(false);
        return true;
    }
    public override bool UniqueRelease()
    {
        base.UniqueRelease();
        ShowHiddenItems(true);
        return true;
    }
    public override bool UniqueCombine()
    {
        base.UniqueCombine();
        AddCharge();
        return true;
    }


    void AddCharge()
    {
        batteryPercentage += 5;
    }

    public void ShowHiddenItems(bool show)
    {
        var allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (var itemObj in allItems)
        {
            Item item = itemObj.GetComponent<Item>();
            if (!item.alwaysNotice)
            {
                if (item.BeingHeld && item.canHoldWithRadio)
                    continue;

                StartCoroutine(item.FadeOut(show));
            }
        }
    }
}
