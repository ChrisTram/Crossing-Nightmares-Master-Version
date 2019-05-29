using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePopupTestingScript : MonoBehaviour {

    // Use this for initialization
    public Camera cam;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                DialoguePopup.Create(hit.point,"Hey, Listen...");
            }
        }
	}
}
