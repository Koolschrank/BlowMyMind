using Player;
using UnityEngine;

namespace Item
{
    public class DefaultItem : Item
    {
        [SerializeField] private HitData hitData;
        [SerializeField] private Collider hitBox;

        public override void Use()
        {
            base.Use();
            Player.PlayAttackAnimation();
        }

        public override void Impact(Collider collider)
        {
            if (!collider.TryGetComponent(out PlayerCharacter nearbyPlayer))
                return;
            
            if (nearbyPlayer == Player)
                return;
            
            Vector3 power = transform.forward * hitData.ForwardForce + transform.up * hitData.UpForce;
            nearbyPlayer.TakeDamage(power, hitData);
            Player.TakeDamage(hitData.SelfDamage);
            hitData.ActivateEffects();
        }

        public override void EnableHitBox()
        {
            hitBox.enabled = true;
        }

        public override void DisableHitBox()
        {
            hitBox.enabled = false;
        }

        protected override void FinishUse()
        {
            base.FinishUse();
            Depleted?.Invoke();
        }
    }
}
