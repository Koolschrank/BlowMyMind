using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private UnityEvent playerDied;
    
    private void Awake()
    {
        var colliders = GetComponentsInChildren<DeathZoneCollider>();
        foreach (var deathZoneCollider in colliders)
        {
            deathZoneCollider.Triggered += HandleTriggerDetected;
        }
    }

    private void HandleTriggerDetected()
    {
        playerDied?.Invoke();
    }
}
