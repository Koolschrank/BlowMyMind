using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
   [SerializeField] private AnimationCurve knockBackFactorByDamage;
   public float maxDamage = 5.0f;

   public DamageData baseAttackData;
   public DamageData hammerData;
   public DamageData featherData;
   
   public float GetKnockBackFactorByDamage(float damage)
   {
      return knockBackFactorByDamage.Evaluate(damage);
   }

   public DamageData GetDamageDataById(int id)
   {
      return id switch
      {
         0 => baseAttackData,
         1 => hammerData,
         2 => featherData,
         _ => throw new ArgumentOutOfRangeException($"No damage data of id {id} is implemented!")
      };
   }
}
