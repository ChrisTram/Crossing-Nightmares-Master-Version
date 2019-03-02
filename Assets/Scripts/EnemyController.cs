using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyController : MonoBehaviour
{


    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public ThirdPersonCharacter player;

    public float dmgPower;

    void Start()
    {
        agent.updateRotation = false;

    }
    private bool firstFHit = true;
    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
            if(firstFHit)
            {
                firstFHit = false;
            }
            else
            {
                player.SendMessage("TakeDamage", dmgPower);
            }

        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.SendMessage("TakeDamage", dmgPower);
        }
    }*/

}
