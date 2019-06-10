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
                if (hit.collider.tag == "Trigger" || hit.collider.tag == "Item")
                {
                    hit.collider.SendMessage("showDialogPopup",hit);
                }
            }
        }
        //On Trigger un objet, cela le fera bouger / disparaitre selon son état
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Trigger")
                {

                    hit.collider.SendMessage("Trigger");
                    Debug.Log("---> Hit: ");
                }
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

    public void startEffect(string effectName)
    {
        //Optimal pour le moment pour mon projet, cela ne serait pas viable de ne travailler qu'avec des ParticleSystem, 
        //je préfère les récupérer quand j'en ai besoin, c'est à dire seulement pour un effet qui persistera
        //Les autres pourront être juste activés puis arrêtés. Si besoin ajout de tags plus tard, Linq useless.
        Debug.Log("startEffect");
        GameObject effect = effects.Find(obj => obj.name == effectName);
        if(effect.activeSelf)
        {
            ParticleSystem effectPS = effect.GetComponent<ParticleSystem>();
            effectPS.Play();
        }
        else
        {
            effect.SetActive(true);
        }
    }

    public void stopEffect(string effectName)
    {
        Debug.Log("stopEffect");
        ParticleSystem effect = effects.Find(obj => obj.name == effectName).GetComponent<ParticleSystem>();
        effect.Stop();
        if (effectName != "Luc")
        {
            effects.Find(obj => obj.name == effectName).SetActive(false); //TODO A CHANGER DEUX FONCTIONS DISTINCTES
        }
    }

}
