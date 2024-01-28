using System;
using Player;
using UnityEngine;

namespace Item
{
    public class PickupItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private ParticleSystem pickupVFX;

        private const float MinYPos = 1.5f;
        private const float RotationSpeed = 1.3f;
        private const float HoverSpeed = 1.5f;
        private const float HoverStrength = 0.2f;

        private bool _isFalling = true;
        private Vector3 _hoverStartPoint;
        private float _hoverTimer;
        
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
            Instantiate(pickupVFX, transform.position, Quaternion.Euler(-90, 0, 0));
            DestroyItem();
        }

        private void Update()
        {
            if (_isFalling && transform.position.y <= MinYPos)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                _isFalling = false;
                _hoverStartPoint = transform.position;
            }else if (!_isFalling)
            {
                transform.RotateAround(Vector3.up, RotationSpeed * Time.deltaTime);
                transform.position = _hoverStartPoint + new Vector3(0, HoverSpeed * Mathf.Sin(_hoverTimer * HoverSpeed) * HoverStrength + HoverStrength, 0);
                _hoverTimer += Time.deltaTime;
            }
        }

        public void DestroyItem()
        {
            Destroy(gameObject);
        }
    }
}
