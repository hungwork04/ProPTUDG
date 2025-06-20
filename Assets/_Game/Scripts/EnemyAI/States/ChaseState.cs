
using UnityEngine;
using BossMap;

namespace StateMachine
{
    public class ChaseState : State<EnemyAI>
    {
        public ChaseState(EnemyAI entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
       
            if (entity.Player == null)
            {
                Debug.LogWarning("Target for enemy is null");
                return;
            }

            entity.CurrentState = this.GetType().Name;
            entity.Target = entity.Player;
            entity.AIDestinationSetter.target = entity.Target;
            entity.AIPath.SearchPath();
        }

        public override void Update()
        {
            base.Update();
            if (entity.AIPath.velocity.x > 0.01f) entity.SetFacing(true);
            if(entity.AIPath.velocity.x < -0.01f) entity.SetFacing(false);
        }
        public override void OnExit()
        {
            base.OnExit();
            entity.AIDestinationSetter.target = null;
            entity.Target = null;
        }
    }

}
