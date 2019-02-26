using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerScript : MonoBehaviour {

    public bool isDamaging;
    public float damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.SendMessage((isDamaging) ? "TakeDamage" : "HealDamage", damage);
        Destroy(gameObject);
    }
}
