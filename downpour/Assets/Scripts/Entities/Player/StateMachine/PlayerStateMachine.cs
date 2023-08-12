using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerStateMachine : StateMachine
    {
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerFallState FallState { get; private set; }

        public Player Player { get; private set; }

        private void Awake() {
            IdleState = new PlayerIdleState(this);
            RunState = new PlayerRunState(this);
            JumpState = new PlayerJumpState(this);
            FallState = new PlayerFallState(this);

            Player = GetComponent<Player>();
        }

        private void Start() {
            EnterDefaultState();
        }

        public void PlayStateAnimation() {
            (CurrentState as PlayerState).PlayStateAnimation();
        }
        
        public bool EnterDefaultState() {
            // Check if current state can't enter default, like falling, wallclimbing, dashing, ect.
            ChangeState(Player.PlayerMovementController.MovingDirection == 0 ? IdleState : RunState);
            return true;
        }

        public bool EnterJumpState() {
            if(!((Player.PlayerMovementController.DesiredJump) && (Player.PlayerMovementController.JumpBufferCounter > 0) && 
                ( (Player.PlayerMovementController.CoyoteCounter > 0) || (!Player.PlayerMovementController.UsedDoubleJump && Player.PlayerData.CurrentPlayerStats.HasDoubleJump && (CurrentState is PlayerFallState)) ))) { // Check for grounded
                return false;
            }

            ChangeState(JumpState);
            return true;
        }

        public bool EnterFallState() {
             if(((Player.PlayerMovementController.DesiredJump) && (Player.PlayerMovementController.JumpBufferCounter > 0) && 
                ( (Player.PlayerMovementController.CoyoteCounter > 0) || (!Player.PlayerMovementController.UsedDoubleJump && Player.PlayerData.CurrentPlayerStats.HasDoubleJump && (CurrentState is PlayerFallState)) ))) {
                return false;
            }
            if(Player.PlayerMovementController.Grounded || Player.PlayerMovementController.rbVelocityY >= 0) { // Check for grounded
                return false;
            }

            ChangeState(FallState);
            return true;
        }
    }
}
