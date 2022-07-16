using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    

    public GameObject customiseScreenHolder;
    private bool customiseScreenState = false;

    private void Start()
    {
        if(customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
    }

    public void ToggleCustomScreen()
    {
        customiseScreenState = !customiseScreenState;
        if(customiseScreenHolder != null) customiseScreenHolder.SetActive(customiseScreenState);
    }
}
