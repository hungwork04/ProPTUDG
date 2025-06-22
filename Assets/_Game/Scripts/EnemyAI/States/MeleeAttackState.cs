
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class MeleeAttackState : State<DemonCtrl>
    {
        public MeleeAttackState(DemonCtrl entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            if (entity.Player == null)
            {
                Debug.LogWarning("Player for boss is null");
                return;
            }
            entity.Target = entity.Player;
            entity.AIPath.canMove = false;
            entity.AIPath.SetPath(null);
        }
        public override void Update()
        {
            base.Update();
            if (entity.Target == null) return;
            Vector3 direction = entity.Target.position - entity.transform.position;
            if(direction.x > 0) entity.SetFacing(true);
            if(direction.x < 0) entity.SetFacing(false);
        }
        public override void OnExit()
        {
            base.OnExit();
            entity.AIPath.canMove = true;
        }

        public override void AnimationTrigger()
        {
            base.AnimationFinishTrigger();
            Attack();
            
        }

        void Attack()
        {
            if (entity.Target == null)
            {
                Debug.LogWarning("target for boss is null");
                return;
            }

            Health health = entity.Target.GetComponent<Health>();
            if(health != null) health.TakeDamage(entity.Damage);

        }
    }

}
