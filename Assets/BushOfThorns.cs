using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushOfThorns : MonoBehaviour
{
    public GameObject thornsPrefab;
    [SerializeField]
    int offset = 5;
    [SerializeField]
    int maxThorns = 5;
    List<GameObject> thorns;

    // Start is called before the first frame update
    void Start()
    {
        thorns = new List<GameObject>();

        CreateThorn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateThorn()
    {
        GameObject newItem;
        newItem = (GameObject)Instantiate(thornsPrefab);
        if (thorns.Count > 0)
        {
            var pos = thorns[thorns.Count - 1].transform.position;
            pos.x += offset;
            newItem.transform.position = pos;
        }
        else
        {
            newItem.transform.position = transform.position;
        }

    }
}
