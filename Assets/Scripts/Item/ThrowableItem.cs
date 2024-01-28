using System.Collections;
using System.Collections.Generic;
using Item;
using Player;
using UnityEngine;

public class ThrowableItem : Item.Item
{
    [SerializeField] private GameObject projectilePrefab;
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
        var projectileRb = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity, null).GetComponent<Rigidbody>();
        projectileRb.AddForce(Player.GetBody().forward * throwForceForward + Vector3.up * throwForceUp);// add body forward
        projectileRb.GetComponent<ThrowableProjectile>().SetOriginalPlayer(Player);
        return true;
    }

    private void FinishUse()
    {
        InUse = false;
        UseCompleted?.Invoke();
        Depleted?.Invoke();
    }
}
