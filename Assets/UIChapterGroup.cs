using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChapterGroup : MonoBehaviour {

    public float width;
    public float height;


    void Start()
    {
        width = this.gameObject.GetComponent<RectTransform>().rect.width;
        height = this.gameObject.GetComponent<RectTransform>().rect.height;

        Vector2 newSize = new Vector2((width / 4)-20, (height / 5)-20);
        this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
