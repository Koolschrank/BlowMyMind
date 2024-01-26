using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : PoolObject
{
    ParticleSystem pSystem;
    public float destroyTime = 2f;

    private void OnEnable()
    {
        if(!CheckPooled())
        {
            pSystem = GetComponent<ParticleSystem>();
            return;
        }
        pSystem.Play();

    }
    
}
