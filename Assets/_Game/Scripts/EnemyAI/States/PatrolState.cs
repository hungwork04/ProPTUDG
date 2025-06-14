
using UnityEngine;
using BossMap;
namespace StateMachine
{
    public class PatrolState : State<EnemyAI>
    {
   
        private int nextWayPoint;
  
    
        public PatrolState(EnemyAI entity, string animBoolName) : base(entity, animBoolName)
        {
            nextWayPoint = -1;
        }

  
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            if (entity.PatrolPosition == null || entity.PatrolPosition.Count == 0)
            {
                Debug.LogWarning("Position patrol for boss is null");
                return;
            }

        
            UpdateWayPoint();
            entity.AIPath.SearchPath();
        }
        private void UpdateWayPoint()
        {
            nextWayPoint = (nextWayPoint + 1) % entity.PatrolPosition.Count;
            entity.Target = entity.PatrolPosition[nextWayPoint];
            entity.AIDestinationSetter.target = entity.PatrolPosition[nextWayPoint];
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
