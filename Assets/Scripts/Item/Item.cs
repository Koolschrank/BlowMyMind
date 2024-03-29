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
        [SerializeField] protected DeathParticles deathParticles;
        
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
    
    // Serializable class for hit with forward force up force and damage
    [Serializable]
    public class HitData
    {
        [SerializeField] float forwardForce = 100f;
        [SerializeField] float upForce = 100f;
        [SerializeField] float damage = 10f;
        [SerializeField] float selfDamage = 0f;
        [SerializeField] SlowDownValue slowDownValues;
        [SerializeField] DamageNumberValue damageNumberValue;
        [SerializeField] SoundEffectValue soundEffectValue;
        // unity event for taking damage
        public UnityEngine.Events.UnityEvent OnTakeDamage;



        public float ForwardForce { get => forwardForce; set => forwardForce = value; }
        public float UpForce { get => upForce; set => upForce = value; }
        public float Damage { get => damage; set => damage = value; }
        public float SelfDamage { get => selfDamage; set => selfDamage = value; }
        
        // get damage number value
        public DamageNumberValue GetDamageNumberValue()
        {
            return damageNumberValue;
        }
        public void ActivateEffects()
        {
            soundEffectValue?.Play();
            slowDownValues?.Play();
            // event
            OnTakeDamage?.Invoke();

        
        }
    }
}
