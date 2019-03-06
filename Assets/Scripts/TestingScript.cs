using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour {

    [SerializeField] Sprite spriteTest;

    // Use this for initialization
    void Start () {
        Item itemTest = new Item("UIRose", spriteTest);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }
}
