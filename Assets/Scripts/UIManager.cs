using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject UITitle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UITitle.SetActive(true);
        }
    }
}
