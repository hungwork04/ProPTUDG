using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMap
{
    public class HealthSystem : ComponentBehavior
    {
        public float maxHP { get; private set; }
        public float curHP { get; private set; }
        public Action OnHPChange;
        public Action OnDead;
        [SerializeField] private bool isDead = false;

     

        public void Init(float hpValue)
        {
            maxHP = hpValue;
            curHP = hpValue;
            isDead = false;
        }

        public void TakeDamage(float damage)
        {
            if(isDead) return;
            float newHP = Mathf.Max(curHP - damage, 0);
            if (Math.Abs(newHP - curHP) > .0001f)
            {
                curHP = newHP;
                OnHPChange?.Invoke();
                
            } 
            if (newHP == 0)
            {
                curHP = 0;
                OnDead?.Invoke();
                isDead = true;
               
            }
           
            
        }
    }

}
