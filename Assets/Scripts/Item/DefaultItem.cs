using Player;
using UnityEngine;

namespace Item
{
    public class DefaultItem : Item
    {
        [SerializeField] private HitData hitData;
        [SerializeField] private float coolDownTime;
        
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
            if (!collider.TryGetComponent(out PlayerCharacter nearbyPlayer))
                return;
                
            if(nearbyPlayer == Player)
                return;
                
            Vector3  power = transform.forward * hitData.ForwardForce + transform.up * hitData.UpForce;
            nearbyPlayer.TakeDamage(power, hitData);
                
            hitData.ActivateEffects();
        }


        
        private void FinishUse()
        {
            InUse = false;
            UseCompleted?.Invoke();
            Depleted?.Invoke();
        }
    }
}
