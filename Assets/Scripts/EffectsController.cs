using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour {

	// Récupération de tous les enfants de premier rang de l'empty effet.
	public List<GameObject> getEffects()
    {
        List<GameObject> effects = new List<GameObject>();

        for (int ID = 0; ID < transform.childCount; ID++)
        {
            effects.Add(transform.GetChild(ID).gameObject);
        }

        return effects;
    }
}
