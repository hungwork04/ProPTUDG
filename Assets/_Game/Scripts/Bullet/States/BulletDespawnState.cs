using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class BulletDespawnState : State<BulletCtrl>
    {
        public BulletDespawnState(BulletCtrl entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            PoolingManager.Despawn(entity.gameObject);
        }
    }

}
