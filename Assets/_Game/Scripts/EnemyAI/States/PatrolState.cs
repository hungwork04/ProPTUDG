
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

        private bool CheckPatrolPathAvailable()
        {
            if (entity.PatrolPosition == null || entity.PatrolPosition.Count == 0) return false;
            foreach (Transform trf in entity.PatrolPosition)
            {
                if (trf == null) return false;
            }

            return true;
        }
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            if (!CheckPatrolPathAvailable())
            {
                entity.PatrolPosition = PatrolPositionManager.Instance.GetPositions(entity.GetType().Name);
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
