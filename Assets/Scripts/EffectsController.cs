using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour {

	// Use this for initialization
	public List<GameObject> getEffects()
    {
        List<GameObject> effects = new List<GameObject>();

        Transform[] Children = new Transform[transform.childCount];
        for (int ID = 0; ID < transform.childCount; ID++)
        {
            effects.Add(transform.GetChild(ID).gameObject);
        }

        return effects;
    }
}
