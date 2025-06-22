using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossMap;
namespace StateMachine
{
    public class RangedAttackState : State<DragonRedCtrl>
    {
        public RangedAttackState(DragonRedCtrl entity, string animBoolName) : base(entity, animBoolName)
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
            base.AnimationTrigger();
            Shoot();
        }

        private void Shoot()
        {
            if (entity.FireBallPrefab == null)
            {
                Debug.LogWarning("fire ball of boss is null");
                return;
            }

            if (entity.Target == null)
            {
                Debug.LogWarning("target for boss is null");
                return;
            }

            BoxCollider2D targetCollider = entity.Target.GetComponent<BoxCollider2D>();
            if (targetCollider == null)
            {
                Debug.LogWarning("player must cotain rigidbody");
                return;
            }

            Vector3 targetPoint = targetCollider.bounds.center;
            
            
           
            Vector3 firePoint = entity.transform.TransformPoint(entity.BossGFX.localScale.x > 0 ? entity.LeftFirePoint : entity.RightFirePoint);

            
            
            Vector3 direction = targetPoint - firePoint;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            BulletCtrl bulletCtrl = PoolingManager.Spawn(entity.FireBallPrefab,firePoint, Quaternion.Euler(0,0,angle)).GetComponent<BulletCtrl>();
            bulletCtrl.Direction = direction.normalized;
        }
    }

}
