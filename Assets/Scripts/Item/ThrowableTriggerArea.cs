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
        [SerializeField] private HitData hitData;
        private void OnTriggerEnter(Collider other)
        {
            if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                triggerEvent?.Invoke();
                throwableProjectile_Parent.PlayerCollision(other);
                var player = other.GetComponent<PlayerCharacter>();



                if (player == null)
                {
                    Debug.LogError($"Throwable Area was enter by collider '{other.name}' which is missing a valid PlayerCharacter component.");
                    return;
                }
               
            }
        }
    }
}
