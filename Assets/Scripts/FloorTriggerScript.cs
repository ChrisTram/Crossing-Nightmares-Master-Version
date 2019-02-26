using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerScript : MonoBehaviour {

    public bool isDamaging;
    public float damage = 1;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            other.SendMessage((isDamaging) ? "TakeDamage" : "HealDamage", Time.deltaTime * damage);
    }
}
