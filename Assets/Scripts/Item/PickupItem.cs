using Player;
using UnityEngine;

namespace Item
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                OnCollected(player);
            }
        }

        private void OnCollected(PlayerCharacter player)
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
