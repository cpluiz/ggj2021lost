using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public string itemName = "New Item";      //    What the item will be called in the inventory
    public Sprite itemIcon = null;         //    What the item will look like in the inventory
    public Rigidbody2D itemObject = null;       //    Optional slot for a PreFab to instantiate when discarding
    public bool isUnique = false;             //    Optional checkbox to indicate that there should only be one of these items per game
    public bool destroyOnUse = false;
    public bool alwaysNotice = true; // Will be used to know if the radio is being held or not
    public bool canHoldWithRadio = true; // Will be used to know if the radio is being held or not
    public bool dualHanded = false;
    [SerializeField]
    int limitUses = -1;

    Quaternion initialRot;
    bool beingHeld;

    public Quaternion HoldingRotation { get => initialRot; }
    public bool BeingHeld { get => beingHeld; set => beingHeld = value; }
    public bool CanUse()
    {
        return limitUses != 0;
    }
    public void Use()
    {
        if (limitUses > 0) --limitUses;
    }

    public void CopyItem(Item item)
    {
        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemObject = item.itemObject;
        isUnique = item.isUnique;
        destroyOnUse = item.destroyOnUse;
        alwaysNotice = item.alwaysNotice;
        canHoldWithRadio = item.canHoldWithRadio;
        dualHanded = item.dualHanded;
    }

    void Awake()
    {
        initialRot = transform.rotation;
        BeingHeld = false;

        FadeOut(alwaysNotice);
    }

    public virtual bool UniqueCatch() { return false; }
    public virtual bool UniqueRelease() { return false; }
    public virtual bool UniqueCombine() { return false; }
    public virtual bool UniqueCombineWithScenario() { return false; }


    public IEnumerator FadeOut(bool show, int afterTime = 0)
    {
        yield return new WaitForSeconds(afterTime);

        //item.SetActive(show); // cant fnd with FindGameObjectsWithTag, so will disable components
        if (TryGetComponent<Rigidbody2D>(out var body))
        {
            body.isKinematic = !show;
            body.freezeRotation = !show;
            if (body.isKinematic)
            {
                body.velocity = Vector2.zero;
                body.angularVelocity = 0;
            }
        }
        if (TryGetComponent<Collider2D>(out var collider))
            collider.enabled = show;
        if (TryGetComponent<SpriteRenderer>(out var renderer))
            renderer.enabled = show;
    }
}