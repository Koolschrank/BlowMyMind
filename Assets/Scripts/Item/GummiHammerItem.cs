using Player;
using UnityEngine;

namespace Item
{
    public class GummiHammerItem : Item
    {
        [SerializeField] private HitData hitData;
        [SerializeField] private float coolDownTime;

        public override void Use()
        {
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
