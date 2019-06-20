using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TriggerScript : MonoBehaviour {
    public string itemToTriggerStr;
    public string key_Name;
    public bool giveAnotherItem = false;
    public Transform itemToDrop;

    public void Trigger()
    {
        if(UISlotsManager.instance.checkItem(itemToTriggerStr))
        {
            UISlotsManager.instance.RemoveItemInSlot(new Item(itemToTriggerStr));
            //On vérifie si le trigger doit drop un objet
            if(giveAnotherItem)
            {
                Debug.Log("Drop Item");
                Instantiate(itemToDrop, gameObject.transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
        
    }

    public void showDialogPopup(RaycastHit hit)
    {
        DialoguePopup.Create(hit.point, key_Name+"DP");
    }
}
