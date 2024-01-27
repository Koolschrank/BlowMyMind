using System;
using UnityEngine;

namespace Item
{
    public class ItemHitBox : MonoBehaviour
    {
        private Item _item;

        private void Awake()
        {
            _item = GetComponentInParent<Item>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // ddebug
            Debug.Log("Item hit box trigger enter");
            _item.Impact(other);
        }
    }
}
