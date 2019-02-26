using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour {

    public Camera cam;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    private List<GameObject> effects = new List<GameObject>(); //Liste des effets possédés par le character


    void Awake()
    {
        //On récupère les effets placés dans l'empty effects, qu'ils soient actifs ou non.
        effects = gameObject.GetComponentInChildren<EffectsController>().getEffects();
    }

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
    }/*

    public void startEffect(string effectname)
    {
        Debug.Log("startEffect");
        switch (effectname)
        {
            case "Luc":
                effects[0].SetActive(true);
                break;
            case "Fire":
                effects[1].SetActive(true);
                break;
            case "Orb":
                effects[2].SetActive(true);
                break;
        }
    }

    public void stopEffect(string effectname)
    {
        Debug.Log("stopEffect");
        switch (effectname)
        {
            case "Luc":
                effects[0].SetActive(false);
                break;
            case "Fire":
                effects[1].SetActive(false);
                break;
            case "Orb":
                effects[2].SetActive(false);
                break;
        }
    }*/

}
