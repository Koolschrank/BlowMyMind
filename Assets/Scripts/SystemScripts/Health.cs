using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class Health : MonoBehaviour
{

    [SerializeField] FloatValue healthPoints;
    // add tooltip
    [Tooltip("time in seconds after taking damage in which the object is invincible")]
    [SerializeField] float hitInvincibilityTime = 0.0f; 

    [SerializeField] HitEffect hitAnimation;
    [SerializeField] HitEffect defeatAnimation;
    [Tooltip("if true, hiteffects will be activated via code when taking damage, if false you can also activate them via unity events")]
    [SerializeField] bool autoConnectEffectsToHealth = true;
    [Tooltip("should be true if this is not a pooled object, should be false if this is a pooled object")]
    [SerializeField] bool destroyOnDefeat = true;
    DamageData lastHit;
    float hitInvincibilityTimer = 0f;


    private void OnEnable()
    {
        healthPoints.Start();
    }
    private void Update()
    {
        var delta = Time.unscaledDeltaTime;
        hitAnimation.UpdateHitAnimation(delta);
        defeatAnimation.UpdateHitAnimation(delta);
        hitInvincibilityTimer -= delta;
    }

    public void TakeDamage(Damage damage)
    {
        // check if hitInvincibilityTimer is over
        if (hitInvincibilityTimer > 0) return;

        // copy damage values into lastHit
        lastHit = new DamageData(damage.DamageData.DamageValue);
        healthPoints.SubtractValue(lastHit.DamageValue);

        if (!healthPoints.IsEmpty() && hitInvincibilityTime>0)
        {
            hitInvincibilityTimer = hitInvincibilityTime;
        }

        if (autoConnectEffectsToHealth)
        {
            if (healthPoints.IsEmpty())
            {
                DestroyObject();
            }
            else
            {
                HitObject();
            }
        }
    }

    public void HitObject()
    {
        if (lastHit != null)
        {
            hitAnimation.Play(transform, lastHit);
            lastHit = null;
        }
        else
        {
            hitAnimation.Play(transform);
        }
    }

    public void DestroyObject()
    {
        if (lastHit != null)
        {
            defeatAnimation.Play(transform, lastHit);
            lastHit = null;
        }
        else
        {
            defeatAnimation.Play(transform);
        }


        if (destroyOnDefeat)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    

}

[Serializable]
public class HitEffect
{
    
    bool isSetup = false;
    [SerializeField] HitFlashValue flash;
    [SerializeField] SlowDownValue slowDown;
    [SerializeField] CameraShakeValue shake;
    [SerializeField] SoundEffectValue sound;
    [SerializeField] PoolSpawnValue spawn_fromPool;
    [SerializeField] SpawnValue spawn;
    [SerializeField] DamageNumberValue text;
    [SerializeField] UnityEvent signal;


    public void SetUpHitAnimation()
    {
        flash.SetUpHitFlash();
        isSetup = true;
    }

    public void Play(Transform transform, int damage)
    {
        Play(transform);
        text.Play(damage);
    }

    public void Play(Transform transform, DamageData damage)
    {
        Play(transform);
        text.Play(damage);
        
    }

    public void Play(Transform transform)
    {
        Play();
        spawn_fromPool.Play(transform);
        spawn.Play(transform);
    }

    public void Play()
    {
        if (!isSetup) SetUpHitAnimation();

        flash.StartHitFlash();
        shake.Play();
        slowDown.Play();
        sound.Play();
        if (signal != null)
        {
            signal?.Invoke();
        }


    }

    public void UpdateHitAnimation(float delta)
    {
        flash.UpdateHitFlash(delta);
    }


}
