using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UISlotsManager : MonoBehaviour {

    List<GameObject> slots;
    public static UISlotsManager instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        slots = new List<GameObject>();
    }

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
        NewSlot.AddComponent<UISlotsClickAction>();
        slots.Add(NewSlot);

    }

    public void RemoveItemInSlot(Item itemToRemove)
    {
        GameObject slotToRemove = slots.Where(obj => obj.name == "UI"+itemToRemove.itemName).FirstOrDefault();
        slots.Remove(slotToRemove);
        Destroy(slotToRemove);
    }
    public void RemoveAllItemInSlot(Item itemToRemove)
    {
        //Same, but removing all items
        List<GameObject> slotsToRemove = slots.Where(obj => obj.name == "UI" + itemToRemove.itemName).ToList<GameObject>();
        foreach (GameObject slot in slotsToRemove)
        {
            slots.Remove(slot);
            Destroy(slot);
        }

        
    }
}
