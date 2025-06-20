
using UnityEngine;
using Utilities;
using BossMap;
namespace StateMachine
{
   
    public class IdleState : State<EnemyAI>
    {
        private CountdownTimer countDownTimer;
        public IdleState(EnemyAI entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
           
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            entity.AIPath.canMove = false;
            entity.AIPath.SetPath(null);
           
            if (entity.TimeIdleDelay <= 0) return;
            countDownTimer = new CountdownTimer(entity.TimeIdleDelay);
            entity.FinishIdleState = false;
            countDownTimer.OnTimerStop += () => entity.FinishIdleState = true;
            countDownTimer.Start();
        }

        public override void Update()
        {
            base.Update();
            if(countDownTimer != null) countDownTimer.Tick(Time.deltaTime);
            else
            {
                Debug.LogWarning("Count down is null");
            }
            
        }

        public override void OnExit()
        {
            base.OnExit();
            entity.AIPath.canMove = true;
        }
    }
}

