using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TriggerScript : MonoBehaviour {
    public string itemToTriggerStr;
    public string key_Name;


    public void Trigger()
    {
        if(UISlotsManager.instance.checkItem(itemToTriggerStr))
        {
            UISlotsManager.instance.RemoveItemInSlot(new Item(itemToTriggerStr));
            Destroy(gameObject);
        }
        
    }

    public void showDialogPopup(RaycastHit hit)
    {
        DialoguePopup.Create(hit.point, key_Name+"DP");
    }
}
