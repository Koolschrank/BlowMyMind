using System;
using UnityEngine;

namespace Item
{
    public class ThrowableProjectile : MonoBehaviour
    {
        [SerializeField] private Collider triggerArea;
        private void OnCollisionEnter(Collision other)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            triggerArea.enabled = true;
        }
    }
}
