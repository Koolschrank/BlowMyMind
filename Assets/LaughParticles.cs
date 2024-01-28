using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughParticles : MonoBehaviour
{
    [SerializeField] private AnimationCurve emissionByDamage;
    public float maxEmission = 50;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void OnDamageChanged(FloatValue newDamage)
    {
        var emission = _particleSystem.emission;
        var newEmissionOverTime = maxEmission * emissionByDamage.Evaluate(newDamage.Value / newDamage.Value_max);
        emission.rateOverTime = newEmissionOverTime;
    }
}
