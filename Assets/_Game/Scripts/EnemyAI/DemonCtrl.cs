


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
        #endregion
        public float Damage { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            IdleState = new IdleState(this, "Idle");
            PatrolState = new PatrolState(this, "Move");
            ChaseState = new ChaseState(this, "Move");
            AttackState = new MeleeAttackState(this, "Attack");
             
            Any(AttackState, new FuncPredicate(IsPlayerInAttackRange));
           
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
            StateMachine.SetState(PatrolState);
            this.AttackRange = 1;
            this.ApproachRange = 1.5f;
            this.Damage = 4;
        }
       
        
    }

}
