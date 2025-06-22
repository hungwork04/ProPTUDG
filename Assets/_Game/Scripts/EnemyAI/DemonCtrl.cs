


using System;
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;
namespace BossMap
{
    public class DemonCtrl : EnemyAI
    {
        #region States

        private IdleState IdleState;
        private PatrolState PatrolState;
        private ChaseState ChaseState;
        private MeleeAttackState AttackState;
        public HurtState HurtState { get; set; }
        public DeadState DeadState { get; set; }
        #endregion
        public float Damage { get; set; }
      
       
        protected override void Awake()
        {
            base.Awake();
            IdleState = new IdleState(this, "Idle");
            PatrolState = new PatrolState(this, "Move");
            ChaseState = new ChaseState(this, "Move");
            AttackState = new MeleeAttackState(this, "Attack");
           
             
            HurtState = new HurtState(this, "Hurt");
            DeadState = new DeadState(this, "Dead");
            
            Any(DeadState, new FuncPredicate(() => OnDead));
            Any(HurtState, new FuncPredicate(() => IsHurt));
            Any(AttackState, new FuncPredicate(IsPlayerInAttackRange));

            At(DeadState, IdleState, new FuncPredicate(() => !OnDead));
            
            At(AttackState, PatrolState, new FuncPredicate(() => !IsPlayerInAttackRange() && !IsPlayerVisible()));
            At(AttackState, ChaseState, new FuncPredicate(() => !IsPlayerInAttackRange() && IsPlayerVisible()));

            
            At(IdleState, PatrolState, new FuncPredicate(() => FinishIdleState && !IsPlayerVisible()));
            At(IdleState, ChaseState, new FuncPredicate(() => FinishIdleState && IsPlayerVisible()));
            At(ChaseState, PatrolState,new FuncPredicate(() => !IsPlayerVisible()));
            At(PatrolState, ChaseState, new FuncPredicate(IsPlayerVisible));
        
            At(PatrolState, IdleState, new FuncPredicate(() => Target != null && Vector2.Distance(Target.position,transform.position) <= 2));


        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(EnemyManager.Instance != null) EnemyManager.Instance.AddEnemy(gameObject);
            Anim.Rebind();
            StateMachine.SetState(IdleState);
          
            this.AttackRange = 1.5f;
            this.ApproachRange = 1.9f;
            this.Damage = 10;
            this.maxHP = 20;
            if (healthSystem != null)
            {
                healthSystem.Init(maxHP);
                healthSystem.OnHPChange = () => IsHurt = true;
                healthSystem.OnDead = () => OnDead = true;
            }
        }

        private void OnDisable()
        {
            if (EnemyManager.Instance != null) EnemyManager.Instance.RemoveEnemy(gameObject);
        }
    }

}
