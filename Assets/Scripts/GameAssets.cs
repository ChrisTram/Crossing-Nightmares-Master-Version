using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOT USED ANYMORE

public class GameAssets : MonoBehaviour {

    private static GameAssets _i;

    public static GameAssets i
    {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameManager"));
            return _i;
        }
    }

    public Transform pfDialoguePopup;
}
