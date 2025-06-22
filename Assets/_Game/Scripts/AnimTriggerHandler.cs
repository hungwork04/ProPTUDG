using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace StateMachine
{
    public class AnimTriggerHandler : ComponentBehavior
    {
        [FormerlySerializedAs("entiy")] [SerializeField] private Entity entity;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (entity == null) entity = GetComponentInParent<Entity>();
        }
        public void AnimationTrigger() =>  entity.StateMachine.State.AnimationTrigger();
        public void AnimationFinishTrigger() => entity.StateMachine.State.AnimationFinishTrigger();
    }

}
