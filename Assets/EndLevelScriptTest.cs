using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScriptTest : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManagerScript.instance.LoadNextScene();
        }
    }
}
