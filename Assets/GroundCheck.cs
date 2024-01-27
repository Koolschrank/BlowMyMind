using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerCaracter playerCaracter;
    public LayerMask groundLayerMask;
    public Collider Collider;
   
    // update
    private void Update()
    {
        // check if player is grounded
        if (IsGrounded())
        {
            // set grounded to true
            playerCaracter.SetGrounded(true);
        }
        else
        {
            playerCaracter.SetGrounded(false);
        }
    }

    // is grounded
    public bool IsGrounded()
    {
        // check if player is grounded
        return Physics.CheckSphere(transform.position, Collider.bounds.extents.x, groundLayerMask);
    }
    

    
}
