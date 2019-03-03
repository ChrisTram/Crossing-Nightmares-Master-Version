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
            float dist = Vector3.Distance(player.gameObject.transform.position, transform.position);
            if (dist <= agent.stoppingDistance)
            {
                    player.SendMessage("TakeDamage", dmgPower);
            }
        }
    }

}
