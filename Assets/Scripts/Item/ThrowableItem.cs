using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class ThrowableItem : Item.Item
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float throwForce;
    public override void Use()
    {
        Invoke(nameof(FinishUse), coolDownTime);
    }

    public override void Impact(Collider collider) { }

    public override bool Throw()
    {
        var projectileRb = Instantiate(projectilePrefab, transform).GetComponent<Rigidbody>();
        //projectileRb.AddForce(Player.transform.forward * throwForce); add body forward
        return true;
    }

    private void FinishUse()
    {
        InUse = false;
        UseCompleted?.Invoke();
        Depleted?.Invoke();
    }
}
