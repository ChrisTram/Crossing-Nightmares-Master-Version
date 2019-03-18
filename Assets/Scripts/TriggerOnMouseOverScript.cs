using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnMouseOverScript : MonoBehaviour {

    void OnMouseOver()
    {
        Debug.Log(this.GetComponent<TriggerScript>().key_Name);
        UIManager.instance.showItemInfo(this.GetComponent<TriggerScript>().key_Name);
    }

    void OnMouseExit()
    {
        UIManager.instance.disableItemInfo();
    }
}
