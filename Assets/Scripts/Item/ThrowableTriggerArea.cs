using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Item
{
    public class ThrowableTriggerArea : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private UnityEvent triggerEvent;
        [SerializeField] ThrowableProjectile throwableProjectile_Parent;
        private void OnTriggerEnter(Collider other)
        {
            if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                triggerEvent?.Invoke();
                throwableProjectile_Parent.PlayerCollision(other);
            }
        }
    }
}
