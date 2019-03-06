using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string name;
    public int slotNb = -1; //-1 Par défaut, n'est donc pas dans un slot UI
    public Sprite sprite; //Le sprite affiché dans l'UI

    public Item(string itemName, Sprite spriteRef)
    {
        name = itemName;
        sprite = spriteRef;
    }

}
