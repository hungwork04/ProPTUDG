
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class BulletExplodeState : State<BulletCtrl>
    {
        public BulletExplodeState(BulletCtrl entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.RB.velocity = Vector3.zero;
            if (entity.Target == null)
            {
                Debug.LogWarning("Target for bullet is null");
                return;
            }

            Health health = entity.Target.GetComponent<Health>();
            if(health != null) health.TakeDamage(entity.Damage);
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.IsDespawn = true;
        }
    }
}


