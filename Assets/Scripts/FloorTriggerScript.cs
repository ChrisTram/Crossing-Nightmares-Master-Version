﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTriggerScript : MonoBehaviour {

    public bool isDamaging;
    public float damage = 1;
    public string effectName;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {

            other.SendMessage((isDamaging) ? "TakeDamage" : "HealDamage", Time.deltaTime * damage);
        } else if (other.tag == "Enemy")
        {
            if (isDamaging) other.SendMessage("TakeDamage", Time.deltaTime * damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (effectName.Length != 0)
            {
                other.SendMessage("startEffect", effectName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (effectName.Length != 0)
            {
                other.SendMessage("stopEffect", effectName);
            }
        }
    }
}
