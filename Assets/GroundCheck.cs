using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundCheck : MonoBehaviour
{
    public PlayerCharacter playerCharacter;
    public LayerMask groundLayerMask;
    public Collider Collider;
   
    // update
    private void Update()
    {
        if (playerCharacter == null) return;
        // check if player is grounded
        if (IsGrounded())
        {
            // set grounded to true
            playerCharacter.SetGrounded(true);
        }
        else
        {
            playerCharacter.SetGrounded(false);
        }
    }

    // is grounded
    public bool IsGrounded()
    {
        // check if player is grounded
        return Physics.CheckSphere(transform.position, Collider.bounds.extents.x, groundLayerMask);
    }
    

    
}
