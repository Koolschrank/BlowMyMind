using System;
using Player;
using UnityEngine;

public class DeathZoneCollider : MonoBehaviour
{
    public Action Triggered;
    private void OnTriggerEnter(Collider other)
    {
        // TODO: Check for Player
        Debug.Log("Collision in DeathZone detected!");


        // check if other is playercaracter
        var playerCaracter = other.GetComponent<PlayerCharacter>();
        if (playerCaracter != null)
        {
            // call triggered event
           playerCaracter.Die();

            // call triggered event
            Triggered?.Invoke();
        }
    }
}
