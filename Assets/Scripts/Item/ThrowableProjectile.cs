using System;
using UnityEngine;

namespace Item
{
    public class ThrowableProjectile : MonoBehaviour
    {
        // collision mask
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private Collider triggerArea;
        [SerializeField] private bool stopsOnCollision;
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Item hit box trigger enter");
            // check if collision is with player
            if (stopsOnCollision && collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                // print
                Debug.Log("Item hit box trigger enter2");
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                triggerArea.enabled = true;
            }


            
        }

        // player collision
        public void PlayerCollision(Collider other)
        {
            Destroy(gameObject);
            return;
        }
    }
}
