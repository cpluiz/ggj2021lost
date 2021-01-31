using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : Item
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
                thorns.gameObject.SendMessage("CallSoakThree");
                return true;
            }
        }
        return false;
    }

}
