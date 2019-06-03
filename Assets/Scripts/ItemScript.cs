using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    public string key_Name;
    public string itemName;
    public Sprite sprite;

    private Item item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            item = new Item(key_Name, itemName, sprite);
            UISlotsManager.instance.AddItemInSlot(item);
            Destroy(gameObject);
        }
    }
    public void showDialogPopup(RaycastHit hit)
    {
        DialoguePopup.Create(hit.point, key_Name + "DP");
    }
}
