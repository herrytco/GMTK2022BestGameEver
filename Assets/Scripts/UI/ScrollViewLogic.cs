using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewLogic : MonoBehaviour
{
    public GameObject scrollableContainerPrefab;
    //private float spacing = 70;

    private void Start()
    {
        if (scrollableContainerPrefab == null) return;
        for (int i = 0; i < 10; i++)
        {
            //GameObject container = Instantiate(scrollableContainerPrefab, transform);
            //container.transform.position -= new Vector3(0, spacing*i);
        }
    }

    void AddNewPlayer()
    {
        GameObject container = Instantiate(scrollableContainerPrefab, transform);
        //modify content
    }
}
