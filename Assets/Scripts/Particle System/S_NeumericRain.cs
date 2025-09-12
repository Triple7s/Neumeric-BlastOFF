using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class S_NeumericRain : MonoBehaviour
{
    [SerializeField] private S_MathManager mathManager;

    [SerializeField] private List<ParticleSystem> targetParticleSystems = new List<ParticleSystem>();


    void Awake()
    {
        // If no particle systems are assigned manually, auto-assign from children
        if (targetParticleSystems.Count == 0)
            targetParticleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
    }

    /*void Start()
    {
        foreach (var ps in targetParticleSystems)
        {
            if (ps == null) continue;
            var emission = ps.emission;
            emission.enabled = false;
        }
    }*/

    void OnEnable()
    {
        if (mathManager != null)
            mathManager.OnCorrectAnswer += PlayAllParticles;
    }

    void OnDisable()
    {
        if (mathManager != null)
            mathManager.OnCorrectAnswer -= PlayAllParticles;
    }

    void PlayAllParticles()
    {
        foreach (var ps in targetParticleSystems)
        {
            if (ps == null) continue;

            ps.Play();

            var emission = ps.emission;
            emission.enabled = true;
        }
    }
}
