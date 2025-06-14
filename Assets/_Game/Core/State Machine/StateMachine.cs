using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        class StateNode
        {
            public IState State { get; }
         
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition, Func<StateData> getstateData = null)
            {
                Transitions.Add(new Transition(to, condition, getstateData));
            }
            
        }

        public IState PreviousState;
        public Func<StateData> PreviousFuncStateData;
        private StateNode current;
        public IState State => current.State;
        private Dictionary<Type, StateNode> nodes = new();
        private HashSet<ITransition> anyTransitions = new();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.To, transition.Data);
           
            
            current.State?.Update();
        }

        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }
        //use for specific condition
        public void SetState(IState state, Func<StateData> stateData = null)
        {
            PreviousState = current?.State;
            PreviousFuncStateData = stateData;
            current = GetOrAddNode(state);
            //current =  nodes[state.GetType()];
            current.State?.OnEnter(stateData?.Invoke());
        }

        public void SetPreviousState()
        {
            if (PreviousState == null)
            {
                Debug.LogWarning("Previous state is null");
                return;
            }
            
            ChangeState(PreviousState, PreviousFuncStateData);
        }

        public void ChangeState(IState state, Func<StateData> stateData = null)
        {
            if (state == current.State) return;

            PreviousState = current.State;
            PreviousFuncStateData = stateData;
            var nextState = GetOrAddNode(state).State;
            //var nextState = nodes[state.GetType()].State;
            
            PreviousState?.OnExit();
            
            nextState?.OnEnter(stateData?.Invoke());
            current = nodes[state.GetType()];
        }

        ITransition GetTransition()
        {
            if (anyTransitions != null)
            {
                foreach (var transition in anyTransitions)
                {
                    if (transition.Condition.Evaluate()) return transition;
                }
            }

            if (current.Transitions != null)
            {
                foreach (var transition in current.Transitions)
                {
                    if (transition.Condition.Evaluate()) return transition;
                }
            }
            

          
           

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition, Func<StateData> getData = null)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition, getData);
        }

        public void AddAnyTransition(IState to, IPredicate condition, Func<StateData> getData = null)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition, getData));
        }
        StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(),node);
            }

            return node;
        }
        
    }
    
    
}
