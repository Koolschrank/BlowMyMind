using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughParticles : MonoBehaviour
{
    [SerializeField] private AnimationCurve emissionByDamage;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void _OnDamageChanged(float newDamage)
    {
        var emission = _particleSystem.emission;
        emission.rateOverTime = emissionByDamage.Evaluate(newDamage);
    }
}
