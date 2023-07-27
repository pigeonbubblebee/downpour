using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity;

namespace Downpour.Entity
{
    public abstract class StateMachine : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        [SerializeField] private bool _logStates;

        private void Update() {
            if(CurrentState != null) {
                CurrentState.Update();
            }
        }

        private void FixedUpdate() {
            if(CurrentState != null) {
                CurrentState.FixedUpdate();
            }
        }

        public void ChangeState(State newState) {
            if(_logStates) {
                Debug.Log(newState);
            }

            State previousState = CurrentState;

            if(CurrentState != null) {
                CurrentState.Exit();
            }

            CurrentState = newState;
            
            CurrentState.Enter(previousState);
        }

    }
}
