using UnityEngine;

namespace Item
{
    public abstract class PickupItem : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCaracter player))
            {
                OnCollected(player);
            }
        }

        protected virtual void OnCollected(PlayerCaracter player)
        {
            //other.GetComponent<PlayerCaracter>().ItemPickUp(itemInt);
            DestroyItem();
        }
    
        public void DestroyItem()
        {
            Destroy(gameObject);
        }
    }
}
