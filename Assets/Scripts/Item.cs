using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public string keyname;
    public string itemName;
    public Sprite sprite; //Le sprite affiché dans l'UI

    public Item(string ItemName, Sprite SpriteRef)
    {
        itemName = ItemName;
        sprite = SpriteRef;
    }

    public Item(string KeyName, string ItemName, Sprite SpriteRef)
    {
        keyname = KeyName;
        itemName = ItemName;
        sprite = SpriteRef;
    }

}
