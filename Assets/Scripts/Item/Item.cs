using System;
using Player;
using UnityEngine;

namespace Item
{
    public abstract class Item : MonoBehaviour
    {
        public Action Depleted;
        public Action UseCompleted;
        
        public bool InUse { get; protected set; }

        protected PlayerCharacter Player;
        
        public abstract void Initialize(PlayerCharacter player);
        public abstract void Use();

        public abstract void Impact(Collider collider);
        
        public virtual void EnableHitBox() {}
        public virtual void DisableHitBox() {}
        public virtual void Throw() {}
    }
    
    // serializeable class for hit with forwarde force up forece and damage
    [Serializable]
    public class HitData
    {
        [SerializeField] float forwardForce = 100f;
        [SerializeField] float upForce = 100f;
        [SerializeField] float damage = 10f;

        public float ForwardForce { get => forwardForce; set => forwardForce = value; }
        public float UpForce { get => upForce; set => upForce = value; }
        public float Damage { get => damage; set => damage = value; }
        
        public void ActivateEffects(){}
    }
}
