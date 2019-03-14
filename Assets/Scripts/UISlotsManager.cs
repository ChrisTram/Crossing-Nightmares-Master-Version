using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class UISlotsManager : MonoBehaviour {

    public static UISlotsManager instance;
    public List<GameObject> slots;

    public GameObject slotsLayout;
    public GameObject slotsGlowLayout;
    public Sprite glowSprite;

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


        //CREATION DU SLOT ITEM
        GameObject NewSlot = new GameObject(); //Create the GameObject
        Image NewImage = NewSlot.AddComponent<Image>(); //Add the Image Component script
        NewImage.name = "UI" + item.itemName;
        NewImage.sprite = item.sprite; //Set the Sprite of the Image Component on the new GameObject
        NewSlot.GetComponent<RectTransform>().SetParent(slotsLayout.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        NewSlot.SetActive(true); //Activate the GameObject
        //CREATION DU SLOT GLOW
        GameObject GlowSlot = new GameObject(); //Create the GameObject
        Image NewImageG = GlowSlot.AddComponent<Image>(); //Add the Image Component script
        NewImageG.name = "UI" + item.itemName;
        NewImageG.sprite = glowSprite; //Set the Sprite of the Image Component on the new GameObject
        NewImageG.raycastTarget = false;
        GlowSlot.GetComponent<RectTransform>().SetParent(slotsGlowLayout.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        //RELIER LE GLOW A LITEM AFIN DE TOUJOURS LE REFERENCER
        NewSlot.AddComponent<UISlotsClickAction>();
        NewSlot.GetComponent<UISlotsClickAction>().glow = GlowSlot;
        GlowSlot.SetActive(false); //Activate the GameObject

        slots.Add(NewSlot);

    }

    public void RemoveItemInSlot(Item itemToRemove)
    {
        Debug.Log("test");
        Debug.Log("nom item : " + itemToRemove.itemName);
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

    public bool checkItem(string itemName)
    {

        GameObject itemToTrigger = instance.slots.Where(obj => obj.name == "UI" + itemName).FirstOrDefault();
        if (itemToTrigger == null)
        {
            Debug.Log("Item non trouvé");
            return false;
        }
        else
        {
            Debug.Log("Triggered !");
            return true;

        }
    }
}
