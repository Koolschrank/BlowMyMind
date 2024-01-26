using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    [Tooltip("is the package that sends the values of the damage to the reciver")]
    [SerializeField] DamageData damageData;
    [SerializeField] LayerMask hitMask;
    [Tooltip("will destroy object if it collides with an object that is not in hitmask")]
    [SerializeField] bool destroyOnOtherHit = true;
    [SerializeField] bool invincible = false;
    [SerializeField] int maxHits = 1;
    
    int currentHitsLeft = 0;

    [SerializeField] CollisionEffect hit_effect;
    [SerializeField] CollisionEffect destroy_effect;


    // get data
    public DamageData DamageData
    {
        get { return damageData; }
        set { damageData = value; }
    }

    // onenable
    private void OnEnable()
    {
        currentHitsLeft = maxHits;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if other has a layer in the hitMask
        if (hitMask == (hitMask | (1 << other.gameObject.layer)))
        {
            
            if (other.GetComponent<Health>())
            {
                Health health = other.GetComponent<Health>();
                health.TakeDamage(this);
                hit_effect.Play(transform);
            }
            if (!invincible)
            {
                currentHitsLeft--;
                if (currentHitsLeft <= 0)
                    gameObject.SetActive(false);
            }
        }
        else
        {
            if (destroyOnOtherHit)
            {
                gameObject.SetActive(false);
            }

        }

    }

    private void OnDisable()
    {
        destroy_effect.Play(transform);
    }


}


// in damage data you can save the values of the damage, like amount, type, origin, etc.
[Serializable]
public class DamageData
{

    

    [SerializeField] float damageValue;

    public DamageData(float damage)
    {
        damageValue = damage;
    }

    // getter and setter
    public float DamageValue
    {
        get { return damageValue; }
        set { damageValue = value; }
    }

    
}

[Serializable]
public class  CollisionEffect
{
    [SerializeField] SlowDownValue slowDown;
    [SerializeField] CameraShakeValue shake;
    [SerializeField] SoundEffectValue sound;
    [SerializeField] SpawnValue hitSpawn;
    [SerializeField] PoolSpawnValue hitSpawn_fromPool;
    [SerializeField] UnityEvent signal;


    public void Play(Transform transform)
    {
        Play();
        hitSpawn.Play(transform);
        hitSpawn_fromPool.Play(transform);
    }
    public void Play()
    {
        if (slowDown != null)
        {
            slowDown.Play();
        }
        if (shake != null)
        {
            shake.Play();
        }
        if (sound != null)
        {
            sound.Play();
        }
        if (signal != null)
        {
            signal?.Invoke();
        }
    }
}