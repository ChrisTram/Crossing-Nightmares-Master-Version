using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{

    public string key;

    // Use this for initialization
    void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

    public void UpdateText()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

}