using System.Collections.Generic;
using UnityEngine;

public class InventoryMngr : MonoBehaviour
{
    public List<Receipt> receipts;
    public UI_Inventory uiInventory;
    public Transform rightHandAttach;
    public SpriteRenderer rightHandRenderer;
    public Transform leftHandAttach;
    public SpriteRenderer leftHandRenderer;

    Item rightHeldItem;
    Item leftHeldItem;
    [SerializeField]
    Radio radio;
    [SerializeField]
    Item lamp;

    // Start is called before the first frame update
    void Start()
    {
        if (uiInventory == null)
            uiInventory = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<UI_Inventory>();

        uiInventory.SetCatchItemFunc(CatchItem);
        uiInventory.SetCombineItems(CombineItems);

        if (rightHeldItem == null || leftHeldItem == null)
        {
            if (radio == null)
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
                {
                    item.TryGetComponent<Radio>(out radio);
                }
            }

            if (radio != null)
            {
                CatchItem(radio);
                radio.ShowHiddenItems(false);
            }
            if (lamp != null)
            {
                CatchItem(lamp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent<Item>(out var newItem))
                    {
                        CatchItem(newItem);
                        break;
                    }
                }
            }
        }
    }

    bool CombineItems(ItemList itemsToCombine)
    {
        foreach (var receipt in receipts)
        {
            if (itemsToCombine.itemList.Count != receipt.neededItems.itemList.Count)
                continue;

            List<Item> tempList = new List<Item>();
            receipt.neededItems.itemList.ForEach(item => tempList.Add(item));

            for (int i = 0; i < receipt.neededItems.itemList.Count; i++)
            {
                if (!itemsToCombine.itemList[i].CanUse())
                {
                    return false;
                }

                var index = tempList.FindIndex(item => item.itemName == itemsToCombine.itemList[i].itemName);
                if (index == -1)
                    break;
                else if (i == (receipt.neededItems.itemList.Count - 1))
                {
                    GameObject newItem = new GameObject();
                    if (receipt.receiptResultPrefab != null)
                    {
                        newItem = (GameObject)Instantiate(receipt.receiptResultPrefab);
                        newItem.transform.position = transform.position;
                    }

                    itemsToCombine.itemList.ForEach(item =>
                    {
                        if (item.isUnique)
                        {
                            item.UniqueCombine();
                        }

                        if (item.destroyOnUse)
                        {
                            if (item == rightHeldItem)
                            {
                                Destroy(rightHeldItem.gameObject);
                                rightHeldItem = null;
                                uiInventory.DeactivateHeldItem(true);
                            }
                            else if (item == leftHeldItem)
                            {
                                Destroy(leftHeldItem.gameObject);
                                leftHeldItem = null;
                                uiInventory.DeactivateHeldItem(false);
                            }
                        }
                    });


                    string itemList = "";
                    itemsToCombine.itemList.ForEach(item => itemList += item.itemName + " ");
                    Debug.Log("Made " + receipt.name + " combining " + itemList);

                    itemsToCombine.itemList[i].Use();
                    if (newItem != null)
                    {
                        if ((rightHeldItem == null || leftHeldItem == null) && newItem.TryGetComponent<Item>(out var item))
                        {
                            CatchItem(item);
                        }
                    }

                    return true;
                }
                else
                    tempList.RemoveAt(index);
            }
        }
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
        uiInventory.IncludeItem(item);
    }

    void RemoveItem(Item item)
    {
        uiInventory.ExcludeItem(item);
    }

    void ReleaseItem(Item item, bool rightHand)
    {
        uiInventory.ReleaseItem(item, rightHand);
        if (rightHand)
        {
            rightHeldItem.transform.SetParent(null);
            rightHeldItem.GetComponent<Rigidbody2D>().gravityScale = 1;
            rightHeldItem.GetComponent<Collider2D>().enabled = true;
            rightHeldItem = null;

            if (leftHeldItem != null)
            {
                Radio tempRadio;
                if (leftHeldItem.TryGetComponent<Radio>(out tempRadio) && !item.alwaysNotice)
                {
                    StartCoroutine(item.FadeOut(false, 2));
                }
            }
        }
        else
        {
            leftHeldItem.transform.SetParent(null);
            leftHeldItem.GetComponent<Rigidbody2D>().gravityScale = 1;
            leftHeldItem.GetComponent<Collider2D>().enabled = true;
            leftHeldItem = null;

            if (rightHeldItem != null)
            {
                Radio tempRadio;
                if (rightHeldItem.TryGetComponent<Radio>(out tempRadio) && !item.alwaysNotice)
                {
                    StartCoroutine(item.FadeOut(false, 2));
                }
            }
        }

        item.BeingHeld = false;
        if (item.isUnique)
            item.UniqueRelease();
    }

    void CatchItem(Item newItem)
    {
        bool caught = false; //might release instead

        if (newItem == rightHeldItem)
        {
            ReleaseItem(rightHeldItem, true);
        }
        else if (newItem == leftHeldItem)
        {
            ReleaseItem(leftHeldItem, false);
        }
        else if (rightHeldItem == null)
        {
            rightHeldItem = newItem;
            if (rightHandAttach != null)
            {
                rightHeldItem.transform.SetParent(rightHandAttach);
                rightHeldItem.transform.position = rightHandAttach.position;
                rightHeldItem.GetComponent<SpriteRenderer>().sortingOrder = rightHandRenderer.sortingOrder;
            }
            else
            {
                rightHeldItem.transform.SetParent(transform);
            }
            var rightItemRig = rightHeldItem.GetComponent<Rigidbody2D>();
            rightItemRig.gravityScale = 0;
            rightItemRig.velocity = Vector2.zero;
            rightItemRig.angularVelocity = 0;

            rightHeldItem.GetComponent<Collider2D>().enabled = false;
            rightHeldItem.transform.rotation = rightHeldItem.HoldingRotation;
            uiInventory.OnCatchItem(rightHeldItem, true);

            rightHeldItem.BeingHeld = true;
            if (rightHeldItem.isUnique)
                rightHeldItem.UniqueCatch();

            caught = true;
        }
        else if (leftHeldItem == null)
        {
            leftHeldItem = newItem;
            if (leftHandAttach != null)
            {
                leftHeldItem.transform.SetParent(leftHandAttach);
                leftHeldItem.transform.position = leftHandAttach.position;
                leftHeldItem.GetComponent<SpriteRenderer>().sortingOrder = leftHandRenderer.sortingOrder;
            }
            else
            {
                leftHeldItem.transform.SetParent(transform);
            }
            var leftItemRig = leftHeldItem.GetComponent<Rigidbody2D>();
            leftItemRig.gravityScale = 0;
            leftItemRig.velocity = Vector2.zero;
            leftItemRig.angularVelocity = 0;

            leftHeldItem.GetComponent<Collider2D>().enabled = false;
            leftHeldItem.transform.rotation = leftHeldItem.HoldingRotation;
            uiInventory.OnCatchItem(leftHeldItem, false);

            leftHeldItem.BeingHeld = true;
            if (leftHeldItem.isUnique)
                leftHeldItem.UniqueCatch();

            caught = true;
        }

        //gonna check if should, there's an item that can't be held with other
        if (caught && rightHeldItem != null && leftHeldItem != null)
        {
            Radio radioRef;
            if (rightHeldItem.TryGetComponent<Radio>(out radioRef) && !leftHeldItem.canHoldWithRadio)
            {
                ReleaseItem(rightHeldItem, true);
            }
            else if (leftHeldItem.TryGetComponent<Radio>(out radioRef) && !rightHeldItem.canHoldWithRadio)
            {
                ReleaseItem(leftHeldItem, false);
            }
        }
    }
}
