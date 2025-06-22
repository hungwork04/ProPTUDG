
using BossMap;
using UnityEngine;

namespace StateMachine
{
    public class HurtState : State<EnemyAI>
    {
        public HurtState(EnemyAI entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            entity.AIPath.canMove = false;
            //entity.AIPath.SetPath(null);
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
           
            entity.IsHurt = false;
            entity.StateMachine.SetPreviousState();
        }
        public override void OnExit()
        {
            base.OnExit();
            entity.AIPath.canMove = true;
        }
    }
}

