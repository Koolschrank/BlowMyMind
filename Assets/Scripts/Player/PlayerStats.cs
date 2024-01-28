using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStats", order = 1)]
    public class PlayerStats : ScriptableObject
    {
        public float maxPlayerDamage = 100.0f;
        public float maxKnockBackMultiplier = 40.0f;
        [SerializeField] private AnimationCurve knockBackMultiplierByDamage;
        public float maxStunDuration = 2.0f;
        [SerializeField] private AnimationCurve stunDurationByDamage;   
        public float GetKnockBackMultiplierByDamage(FloatValue damage)
        {
            return maxKnockBackMultiplier * knockBackMultiplierByDamage.Evaluate(damage.Value / damage.Value_max);
        }
        
        public float GetStunDurationByDamage(FloatValue damage)
        {
            return maxStunDuration * stunDurationByDamage.Evaluate(damage.Value / damage.Value_max);
        }
        
        
    }
}
