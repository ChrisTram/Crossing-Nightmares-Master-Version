using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemTriggerScript : MonoBehaviour {
    public string itemToTriggerStr;
    public string key_Name;


    public void Trigger()
    {
        if(UISlotsManager.instance.checkItem(itemToTriggerStr))
        {
            UISlotsManager.instance.RemoveItemInSlot(new Item(itemToTriggerStr));
            Destroy(gameObject);
        }


        /*
        List<GameObject> listItems;
        listItems = UISlotsManager.instance.slots;
        
        GameObject itemToTrigger = listItems.Where(obj => obj.name == "UI" + itemToTriggerStr).FirstOrDefault();
        if (itemToTrigger == null)
        {
            Debug.Log("Item non trouvé");
        } else
        {
            Debug.Log("Triggered !");
            UISlotsManager.instance.RemoveItemInSlot(new Item(itemToTrigger.name));
            Destroy(gameObject);
            
        }*/

    }
}
