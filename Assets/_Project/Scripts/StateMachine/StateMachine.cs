using System;
using System.Collections.Generic;

namespace Platformer
{
    public class StateMachine
    {
        private StateNode current;
        private Dictionary<Type, StateNode> nodes = new();
        HashSet<iTransition> anyTransitions = new();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            current.State?.Update();
        }
        
        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        public void SetState(iState state)
        {
            current = nodes[state.GetType()];
            current.State?.onEnter();
        }

        void ChangeState(iState state)
        {
            if(state == current.State) return;
            
            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;
            
            previousState?.onExit();
            nextState?.onEnter();
            current = nodes[state.GetType()];
        }

        iTransition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            foreach (var transition in current.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }
            return null;
        }

        public void AddTransition(iState from, iState to, iPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        
        public void AddAnyTransition(iState to, iPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(iState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            
            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }
            return node;
        }
        class StateNode
        {
            public iState State { get; }
            
            public HashSet<iTransition> Transitions { get; }
            
            public StateNode(iState state)
            {
                State = state;
                Transitions = new HashSet<iTransition>();
            }

            public void AddTransition(iState to, iPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}