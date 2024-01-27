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
            _item.Impact(other);
        }
    }
}
