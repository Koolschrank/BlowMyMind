using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Item
{
    public class ChickenProjectile : MonoBehaviour
    {
        public PlayerCharacter Player { get; set; }
        
        [SerializeField] HitData hitData;

        // ontrigger enter
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out PlayerCharacter player)) 
                return;
            
            if(player != Player)
            {
                PlayerCollision(player);
            }
            
            Invoke(nameof(DestroySelf), 7f);
        }
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out PlayerCharacter player)) 
                return;
            
            if(player != Player)    
            {
                PlayerCollision(player);
            }
            
            Invoke(nameof(DestroySelf), 7f);
        }
        
        public void PlayerCollision(PlayerCharacter player)
        {
            var positionDiff = player.transform.position - Player.transform.position;

            Vector3 power = positionDiff * hitData.ForwardForce + Vector3.up * hitData.UpForce;
            player.TakeDamage(power, hitData);
            Destroy(gameObject);
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
