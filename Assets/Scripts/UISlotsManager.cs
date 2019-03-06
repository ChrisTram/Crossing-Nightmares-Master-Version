using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UISlotsManager : MonoBehaviour {

    List<GameObject> slots;
    //private int freeSlot = 0;
    public void AddItemInSlot(Item item)
    {
        if (item == null)
            return;

        GameObject NewSlot = new GameObject(); //Create the GameObject
        Image NewImage = NewSlot.AddComponent<Image>(); //Add the Image Component script
        NewImage.name = "UI" + item.itemName;
        NewImage.sprite = item.sprite; //Set the Sprite of the Image Component on the new GameObject
        NewSlot.GetComponent<RectTransform>().SetParent(this.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        NewSlot.SetActive(true); //Activate the GameObject
        slots.Add(NewSlot);


        /*
        Image newSlot = new Image();
        slots[freeSlot].GetComponent<Image>().sprite = item.sprite;
        slots[free]
        item.slotNb = freeSlot; 
        freeSlot++;*/
    }

    public void RemoveItemInSlot(Item itemToRemove)
    {
        //Find the item
        GameObject slotToRemove = slots.Where(obj => obj.name == "UI"+itemToRemove.itemName).SingleOrDefault();
        Destroy(slotToRemove);

        //Remove the item
        //remove 1 from slotNb for all the item after itemToRemove.slotNb
    }
    public void RemoveAllItemInSlot(Item itemToRemove)
    {
        //Same, but removing all items (se .ToList() instead of SingleOrDefault() and that will return a collection of all objects named "Sword")
        List<GameObject> slotsToRemove = slots.Where(obj => obj.name == "UI" + itemToRemove.itemName).ToList<GameObject>();
        foreach (GameObject slot in slotsToRemove)
        {
            Destroy(slot);
        }

        
    }
}
