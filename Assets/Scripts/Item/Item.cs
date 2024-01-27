using System;
using Player;
using UnityEngine;

namespace Item
{
    public abstract class Item : MonoBehaviour
    {
        public Action Depleted;
        public Action UseCompleted;
        
        [SerializeField] protected float coolDownTime;
        
        public bool InUse { get; protected set; }

        protected PlayerCharacter Player;

        protected void Awake()
        {
            DisableHitBox();
        }

        public void Initialize(PlayerCharacter player)
        {
            Player = player;
        }

        public virtual void Use()
        {
            InUse = true;
            Invoke(nameof(FinishUse), coolDownTime);
        }

        protected virtual void FinishUse()
        {
            InUse = false;
            UseCompleted?.Invoke();
        }

        public abstract void Impact(Collider collider);
        
        public virtual void EnableHitBox() {}
        public virtual void DisableHitBox() {}

        public virtual bool Throw()
        {
            return false;
        }
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
