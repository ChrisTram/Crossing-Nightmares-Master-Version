using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnMouseOverScript : MonoBehaviour {

    void OnMouseOver()
    {
        Debug.Log(this.GetComponent<ItemScript>().key_Name);
        UIManager.instance.showItemInfo(this.GetComponent<ItemScript>().key_Name);
    }

    void OnMouseExit()
    {
        UIManager.instance.disableItemInfo();
    }
}
