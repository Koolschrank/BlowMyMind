using System;

using UnityEngine;

public class DeathZoneCollider : MonoBehaviour
{
    public Action Triggered;
    private void OnTriggerEnter(Collider other)
    {
        // TODO: Check for Player
        Debug.Log("Collision in DeathZone detected!");
        Triggered?.Invoke();
    }
}
