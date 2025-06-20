using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class SpawnState : State<DragonRedCtrl>
    {
        public SpawnState(DragonRedCtrl entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.CurrentState = this.GetType().Name;
            entity.AIPath.canMove = false;
            entity.AIPath.SetPath(null);

            if (entity.SpawnPositionAvailables == null || entity.SpawnPositionAvailables.Count <= 0)
            {
                Debug.LogWarning("Spawn positions is null");
                return;
            }
            Spawn();
            
        }

        private void Spawn()
        {
            if (entity.DemonPrefab == null)
            {
                Debug.LogWarning("Demon prefab is null");
                return;
            }
            for (int i = 0; i < entity.SpawnAmount; ++i)
            {
                Vector3 spawnPos = Vector3.zero;
                if(!GetPosition(ref spawnPos)) continue;
                PoolingManager.Spawn(entity.DemonPrefab, spawnPos);
            }
        }

        private bool GetPosition(ref Vector3 spawnPos)
        {
            for (int i = 0; i < entity.SpawnPositionAvailables.Count; ++i)
            {
                spawnPos = entity.SpawnPositionAvailables[i];
                spawnPos = entity.transform.TransformPoint(spawnPos);
                Collider2D hit = Physics2D.OverlapCircle(spawnPos, 1);
                
                
                if (hit == null || !hit.tag.Equals("Player") && !hit.tag.Equals("Enemy") && !hit.tag.Equals("Ground")) return true;
            }

            return false;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            
            entity.IsSpawn = false;
            entity.StateMachine.SetPreviousState();
        }
        

        public override void OnExit()
        {
            base.OnExit();
            entity.AIPath.canMove = false;
            if(entity.SpawnTimeCountDown != null) entity.SpawnTimeCountDown.Start();
        }
    }
}


