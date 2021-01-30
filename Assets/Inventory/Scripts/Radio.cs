using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Radio : Item
{
    [SerializeField]
    int batteryPercentage = 0;

    public void AddCharge(int charge)
    {
        batteryPercentage += charge;
    }
}
