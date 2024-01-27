using System.Collections;
using System.Collections.Generic;
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
        var projectileRb = Instantiate(projectilePrefab, transform).GetComponent<Rigidbody>();

        projectileRb.transform.parent = null;
        projectileRb.transform.position += Vector3.up * 0.5f;
        projectileRb.AddForce(Player.GetBody().forward * throwForceForward + Player.GetBody().up * throwForceUp);// add body forward
        return true;
    }

    private void FinishUse()
    {
        InUse = false;
        UseCompleted?.Invoke();
        Depleted?.Invoke();
    }
}
