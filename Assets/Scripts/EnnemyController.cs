using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnnemyController : MonoBehaviour
{


    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public ThirdPersonCharacter player;

    void Start()
    {
        agent.updateRotation = false;
    }

    void LateUpdate()
    {
        agent.SetDestination(player.transform.position);
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }
    }
}