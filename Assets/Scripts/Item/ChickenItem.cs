using UnityEngine;

namespace Item
{
    public class ChickenItem : Item
    {
        [SerializeField] private ChickenProjectile projectilePrefab;
        [SerializeField] private float throwForceForward;
        [SerializeField] private float throwForceUp;
        public override void Use()
        {
            base.Use();
            Player.PlayAttackAnimation();
        }

        public override void Impact(Collider collider) { }

        public override bool Throw()
        {
            var projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity, null);
            projectile.Player = Player;
            var projectileRb = projectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(Player.GetBody().forward * throwForceForward + Vector3.up * throwForceUp);// add body forward
            return true;
        }

        protected override void FinishUse()
        {
            base.FinishUse();
            Depleted?.Invoke();
        }
    }
}
