using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    public Material materialRespawn;
    public Material materialFinal;

    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;

    int shaderProperty;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        _renderer.sharedMaterial = materialRespawn;
        var main = ps.main;
        main.duration = spawnEffectTime;

        ps.Play();

    }

    void Update()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //Fin de l'animation de Spawn
        }
        if (!(timer < spawnEffectTime / 4 + pause))
        {
            _renderer.sharedMaterial = materialFinal; //A partir de ce moment on peut rendre l'apparence basique du personnage
        }

        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));

    }
}
