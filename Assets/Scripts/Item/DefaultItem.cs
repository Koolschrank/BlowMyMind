using Player;
using UnityEngine;

namespace Item
{
    public class DefaultItem : Item
    {
        [SerializeField] private HitData hitData;
        [SerializeField] private float coolDownTime;
        [SerializeField] private Collider hitBox;
        
        public override void Initialize(PlayerCharacter player)
        {
            Player = player;
        }

        public override void Use()
        {
            Player.PlayAttackAnimation();
            Invoke(nameof(FinishUse), coolDownTime);
        }

        public override void Impact(Collider collider)
        {
            // debug
            Debug.Log("Item impact/3 ");
            if (!collider.TryGetComponent(out PlayerCharacter nearbyPlayer))
                return;
            Debug.Log("Item impact/2 ");
            if (nearbyPlayer == Player)
                return;

            Debug.Log("Item impact");
            Vector3  power = transform.forward * hitData.ForwardForce + transform.up * hitData.UpForce;
            nearbyPlayer.TakeDamage(power, hitData);
                
            hitData.ActivateEffects();
        }

        public override void EnableHitBox()
        {
            Debug.Log("Enable hit box");
            hitBox.enabled = true;
        }

        public override void DisableHitBox()
        {
            Debug.Log("Disable hit box");
            hitBox.enabled = false;
        }

        private void FinishUse()
        {
            InUse = false;
            UseCompleted?.Invoke();
            Depleted?.Invoke();
        }
    }
}
