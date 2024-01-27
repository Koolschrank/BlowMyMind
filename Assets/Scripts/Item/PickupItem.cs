using Player;
using UnityEngine;

namespace Item
{
    public abstract class PickupItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                OnCollected(player);
            }
        }

        protected virtual void OnCollected(PlayerCharacter player)
        {
            player.PickUpItem(item);
            DestroyItem();
        }
    
        public void DestroyItem()
        {
            Destroy(gameObject);
            
        }
    }
}
