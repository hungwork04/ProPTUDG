

using System;

using UnityEngine;

namespace StateMachine
{
    public interface IEntity
    {
        public StateMachine StateMachine { get; set; }
        public Animator Anim { get; set; }
        public bool IsAnimationTriggerFinished { get; set; }
        
        
    }

    public class Entity : ComponentBehavior, IEntity
    {
        
        private Animator anim;


        public StateMachine StateMachine { get; set; }

        public Animator Anim
        {
            get => anim;
            set => anim = value;
        }
        public bool IsAnimationTriggerFinished { get; set; }
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (anim == null)
                anim = GetComponentInChildren<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
            
        }
        protected void At(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddTransition(from, to, condition, getData);
        protected void Any(IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddAnyTransition(to, condition, getData);

        

        protected virtual void Update() => StateMachine.Update();

        protected void FixedUpdate() => StateMachine.FixedUpdate();
    }
}