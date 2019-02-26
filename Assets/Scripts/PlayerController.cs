using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour {

    public Camera cam;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    [SerializeField] float increaseHealth = 1f;
    [SerializeField] float Health = 100;

    void Start ()
    {
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        } else
        {
            character.Move(Vector3.zero, false, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "HealZone")
        {
            Debug.Log("HealZone");
        }
        if (other.gameObject.transform.tag == "Heal")
        {
            Debug.Log("Heal");
        }
    }
    private float test =0;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.tag == "HealZone")
        {
            test += Time.deltaTime * 1f;
            Debug.Log(test);

        }
    }
    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}
