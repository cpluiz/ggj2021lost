using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovement : MonoBehaviour
{
    public Transform targetSpot;
    public float speed = 1;
    public float offset = 1;

    void Start()
    {
    }

    void Update()
    {
        transform.position = targetSpot.position + new Vector3(Mathf.Cos(Time.time) * speed, Mathf.Sin(Time.time) * speed, 0.0f);
    }
}
