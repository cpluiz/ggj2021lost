using System.Collections.Generic;
using UnityEngine;

public class InventoryMngr : MonoBehaviour
{
    public Collider2D nearbyItemsCollider;
    public List<Receipt> receipts;
    public UI_Inventory uiInventoryInstance;

    Item heldItem;
    ItemList nearbyItems;
    int maxNearbyItems = 5;

    // Start is called before the first frame update
    void Start()
    {
        nearbyItems = ItemList.Create("PlayerInventory");
        uiInventoryInstance.SetCatchItemFunc(CatchItem);
        uiInventoryInstance.SetCombineItems(CombineItems);

        if (nearbyItemsCollider == null)
        {
            if (!TryGetComponent<Collider2D>(out nearbyItemsCollider))
            {
                nearbyItemsCollider = new CircleCollider2D();
                ((CircleCollider2D)nearbyItemsCollider).radius = .2f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var tempList = new ItemList();
            nearbyItems.itemList.ForEach(nearbyItem => tempList.itemList.Add(nearbyItem));
            if (heldItem != null)
                tempList.itemList.Add(heldItem);

            CombineItems(tempList);
            FindPossibleReceipts(tempList);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.TryGetComponent<Item>(out Item item))
            if (nearbyItems.itemList.Count < maxNearbyItems)
                AddItem(item);
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        if (collider.transform.TryGetComponent<Item>(out Item item))
            if (nearbyItems.itemList.Contains(item))
                RemoveItem(item);
    }

    bool CombineItems(ItemList itemsToCombine)
    {
        foreach (var receipt in receipts)
        {
            if (itemsToCombine.itemList.Count != receipt.neededItems.itemList.Count)
                continue;

            for (int i = 0; i < receipt.neededItems.itemList.Count; i++)
            {
                if (itemsToCombine.itemList.FindIndex(item => item.name == receipt.neededItems.itemList[i].name) == -1)
                    break;

                if (i == (receipt.neededItems.itemList.Count - 1))
                {
                    if (receipt.receiptResultPrefab != null)
                    {
                        GameObject newItem;
                        newItem = (GameObject)Instantiate(receipt.receiptResultPrefab);
                        newItem.transform.position = transform.position;
                    }

                    itemsToCombine.itemList.ForEach(item =>
                    {
                        if(item.isUnique)
                        {
                            if(item.GetComponent<Radio>() != null)
                            {
                                Debug.Log("recarregue");
                                item.GetComponent<Radio>().AddCharge(10);
                            }
                        }

                        if (item.destroyOnUse)
                        {
                            if (item == heldItem)
                            {
                                Destroy(heldItem.gameObject);
                                uiInventoryInstance.DeactivateHeldItem();
                            }

                            var nearbyIndex = nearbyItems.itemList.FindIndex(nearbyItem => nearbyItem.itemName == item.itemName);
                            if (nearbyIndex != -1)
                            {
                                Destroy(nearbyItems.itemList[nearbyIndex]);
                                Destroy(item.gameObject);
                            }
                        }
                    });

                    return true;
                }
            }
        }
        Debug.Log("Can't do receipt");
        return false;
    }

    List<Receipt> FindPossibleReceipts(ItemList itemsToCombine)
    {
        List<Receipt> possibleReceipts = new List<Receipt>();

        foreach (var receipt in receipts)
        {
            if (itemsToCombine.itemList.Count < receipt.neededItems.itemList.Count)
                continue;

            for (int i = 0; i < receipt.neededItems.itemList.Count; i++)
            {
                if (itemsToCombine.itemList.FindIndex(item => item.name == receipt.neededItems.itemList[i].name) == -1)
                    break;

                if (i == (receipt.neededItems.itemList.Count - 1))
                {
                    possibleReceipts.Add(receipt);
                }
            }
        }

        Debug.Log("Can do " + possibleReceipts.Count + " receipts");
        return possibleReceipts;
    }

    void AddItem(Item item)
    {
        nearbyItems.itemList.Add(item);
        uiInventoryInstance.IncludeItem(item);
    }

    void RemoveItem(Item item)
    {
        nearbyItems.itemList.Remove(item);
        uiInventoryInstance.ExcludeItem(item);
    }

    void ReleaseItem(Item item)
    {
        nearbyItems.itemList.Add(item);
        uiInventoryInstance.ReleaseItem(item);
        heldItem.transform.SetParent(null);
        heldItem = null;
    }

    void CatchItem(Item newItem)
    {
        if (newItem == heldItem)
        {
            ReleaseItem(heldItem);
            return;
        }

        var itemIndex = nearbyItems.itemList.FindIndex(item => item.itemName == newItem.itemName);
        if (itemIndex != -1)
        {
            Item oldItem = null;
            if (heldItem != null)
            {
                oldItem = heldItem;
                nearbyItems.itemList.Add(heldItem);
            }
            heldItem = nearbyItems.itemList[itemIndex];
            heldItem.transform.SetParent(transform);

            nearbyItems.itemList.RemoveAt(itemIndex);
            uiInventoryInstance.OnCatchItem(heldItem, oldItem);
        }
    }
}
