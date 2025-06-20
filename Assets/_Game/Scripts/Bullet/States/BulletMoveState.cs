using System.Collections;
using System.Collections.Generic;
using BossMap;
using StateMachine;
using UnityEngine;

namespace BossMap
{
    public class BulletMoveState : State<BulletCtrl>
    {
        public BulletMoveState(BulletCtrl entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            entity.RB.velocity = entity.Direction * entity.Speed;
        }
    }
}

