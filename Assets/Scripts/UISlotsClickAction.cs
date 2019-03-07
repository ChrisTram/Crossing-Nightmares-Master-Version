﻿using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UISlotsClickAction : MonoBehaviour, IPointerClickHandler
{
    public GameObject glow;
    public bool isSelect = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log(name.Substring(2));
            isSelect = !isSelect;
            if (isSelect)
            {
                glow.SetActive(true);
            } 
            else
            {
                glow.SetActive(false);
            }
        }
    }
}