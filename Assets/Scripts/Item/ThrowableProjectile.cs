using Player;
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
        [SerializeField] HitData damageData;
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
            // check if collision is with player
            if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                // print
                PlayerCharacter player = other.GetComponent<PlayerCharacter>();
                if (player == null)
                {
                    Debug.LogError($"Throwable Area was enter by collider '{other.name}' which is missing a valid PlayerCharacter component.");
                    return;
                }
                var positionDiff = player.transform.position - transform.position;
                Vector3 power = positionDiff * damageData.ForwardForce + transform.up * damageData.UpForce;
                player.TakeDamage(power, damageData);
            }
            Destroy(gameObject);
            return;
        }
    }
}
