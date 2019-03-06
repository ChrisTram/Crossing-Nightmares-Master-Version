using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    public string key_Name;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        Destroy(gameObject);
    }
}
