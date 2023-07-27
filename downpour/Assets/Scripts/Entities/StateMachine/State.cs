using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity;

namespace Downpour.Entity
{
    public abstract class State
    {
        protected StateMachine _sm;

        public State(StateMachine stateMachine) {
            _sm = stateMachine;
        }

        public virtual void Enter(State previousState) {
            
        }

        public virtual void Update() {

        }

        public virtual void FixedUpdate() {

        }

        public virtual void Exit() {

        }
    }
}
