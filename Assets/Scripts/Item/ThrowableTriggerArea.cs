using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Item
{
    public class ThrowableTriggerArea : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggerEvent; 
        private void OnTriggerEnter(Collider other)
        {
            if (TryGetComponent(out PlayerCharacter player))
            {
                triggerEvent?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
