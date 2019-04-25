using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterNumber : MonoBehaviour {

    void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = gameObject.transform.parent.name;
    }
}
