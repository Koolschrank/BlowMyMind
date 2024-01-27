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

    public void OnDamageChanged(FloatValue newDamage)
    {
        var emission = _particleSystem.emission;
        emission.rateOverTime = emissionByDamage.Evaluate(newDamage.Value);
    }
}
