using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Machete : Item
{
    public override bool UniqueCombineWithScenario()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        foreach (var hitInfo in hits)
        {
            if (hitInfo.collider.gameObject.TryGetComponent<Thorns>(out var thorns))
            {
                thorns.gameObject.SendMessage("CallDestroyThorns", 5);
                return true;
            }
        }
        return false;
    }
}
