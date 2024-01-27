using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultItem : Item.Item
{
    public override void Initialize(PlayerCaracter player)
    {
        throw new System.NotImplementedException();
    }

    public override void Use()
    {
        // add force to everything in the hitbox
        Collider[] colliders = Physics.OverlapBox(hitBox.bounds.center, hitBox.bounds.extents, hitBox.transform.rotation, hitBoxLayerMask);
        Debug.Log(colliders.Length);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                continue;
            }
            // if rb is this rb, skip
            if (rb == this.rb)
            {
                continue;
            }
            if (rb != null)
            {
                //rb.AddForce(transform.forward * pushForwadForce);
                //rb.AddForce(transform.up * pushupForce);
                // slow down value
                slowDownValue.Play();
                // sound effect value
                soundEffectValue.Play();
                // unity event in not empty
                if (OnAttack != null)
                {
                    OnAttack.Invoke();
                }
                // if other rb has a playerCharacter component
                PlayerCaracter playerCaracter = rb.GetComponent<PlayerCaracter>();
                if (playerCaracter != null)
                {
                    Vector3  power = transform.forward * BaseDamage.ForwardForce + transform.up * BaseDamage.UpForce;
                    // take damage
                    playerCaracter.TakeDamage(power, BaseDamage);
                }
            }
        }
    }
}
