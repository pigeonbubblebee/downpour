using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.Entity.Player
{
    public abstract class PlayerState : State {
        protected PlayerStateMachine _psm => (_sm as PlayerStateMachine);
        protected Player _player => _psm.Player;

        protected PlayerData _playerData => _player.PlayerData;
        protected PlayerMovementController _playerMovementController => _player.PlayerMovementController;
        protected PlayerAnimationController _playerAnimationController => _player.PlayerAnimationController;
        
        public PlayerState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter(State previousState) {
            PlayStateAnimation();
        }

        public virtual void PlayStateAnimation() {

        }
    }

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter(State previousState) {
            base.Enter(previousState);
            _playerMovementController.setVelocity(0, _playerMovementController.rbVelocityY);
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.IdleAnimationClip);
        }

        public override void Update() {
            if(_psm.EnterJumpState()
            || _psm.EnterFallState()) {
                return;
            }

            if(_playerMovementController.MovingDirection != 0) {
                _psm.ChangeState(_psm.RunState);
            }
        }
    }

    public class PlayerRunState : PlayerState
    {
        public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { 
            RunStateRunLogicHandler = new RunLogicHandler(playerStateMachine);
        }

        public RunLogicHandler RunStateRunLogicHandler { get; private set; }

        public override void Enter(State previousState) {
            base.Enter(previousState);
            
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.RunAnimationClip);
        }

        public override void Update() {
            if(_psm.EnterJumpState()
            || _psm.EnterFallState()) {
                return;
            }

            if(_playerMovementController.MovingDirection == 0) {
                _psm.ChangeState(_psm.IdleState);
            } else {
                RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerData);
            }
        }

        public override void FixedUpdate() {
            _playerMovementController.setVelocity(RunStateRunLogicHandler.GetVelocityX(), _playerMovementController.rbVelocityY);
        }

        public class RunLogicHandler {
            private Vector2 _direction, _desiredVelocity;
            private PlayerStateMachine _playerStateMachine;

            public RunLogicHandler(PlayerStateMachine playerStateMachine) {
                _playerStateMachine = playerStateMachine;
            }

            public void SetDesiredVelocity(PlayerMovementController playerMovementController, PlayerData playerData) {
                _direction.x = playerMovementController.MovingDirection;
                _desiredVelocity = new Vector2(_direction.x * playerData.CurrentPlayerStats.MoveSpeed, 0f);
            }

            public float GetVelocityX() {
                return _desiredVelocity.x;
            }
        }
    }

    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {
            JumpStateJumpLogicHandler = new JumpLogicHandler(playerStateMachine);
        }

        public JumpLogicHandler JumpStateJumpLogicHandler { get; private set; }

        private Vector2 _velocity;

        public override void Enter(State previousState) {
            base.Enter(previousState);
            
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.JumpAnimationClip);
        }

        public override void Update() {
            _psm.RunState.RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerData);
        }

        public override void FixedUpdate() {
            _velocity = _playerMovementController.rbVelocity;
            _velocity.x = _psm.RunState.RunStateRunLogicHandler.GetVelocityX();

            _velocity.y = JumpStateJumpLogicHandler.HandleJump(_playerMovementController, _playerData).y;

            if(!_playerMovementController.DesiredJump) {
                _psm.EnterDefaultState();
            }

            _playerMovementController.setVelocity(_velocity);

            if(_psm.EnterFallState()) {
                return;
            }
        }

        public class JumpLogicHandler {
            private float _jumpSpeed;
            private bool _isJumping;
            private Vector2 _velocity;
            private PlayerStateMachine _playerStateMachine;
            public JumpLogicHandler(PlayerStateMachine playerStateMachine) {
                _playerStateMachine = playerStateMachine;
            }
            public Vector2 HandleJump(PlayerMovementController _playerMovementController, PlayerData _playerData) {
                _velocity = _playerMovementController.rbVelocity;

                if(_playerMovementController.Grounded) {
                    _isJumping = false;
                }

                if(_playerMovementController.DesiredJump) {
                    if(_playerMovementController.JumpBufferCounter > 0) {
                        _jumpAction(_playerMovementController, _playerData);
                    }
                } else {
                    _velocity = new Vector2(_velocity.x, 0.075f);
                }

                return _velocity;
            }

            private void _jumpAction(PlayerMovementController _playerMovementController, PlayerData _playerData) {
                if(_playerMovementController.CoyoteCounter > 0 
                || (!_playerMovementController.UsedDoubleJump && _playerData.CurrentPlayerStats.HasDoubleJump && _isJumping)) {
                    if(_isJumping) {
                        _playerMovementController.UseDoubleJump();
                        Debug.Log("Double Jump");
                    }

                    _playerMovementController.ResetCoyoteTime();
                    _playerMovementController.ResetJumpBuffer();

                    _jumpSpeed = MathF.Sqrt(-2f * Physics2D.gravity.y * _playerData.CurrentPlayerStats.JumpHeight);
                    _isJumping = true;

                    if(_playerMovementController.rbVelocityY > 0f) {
                        _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                    }
                    else if (_velocity.y < 0f)
                    {
                        _jumpSpeed += Mathf.Abs(_playerMovementController.rbVelocityY);
                    }

                    _velocity.y += _jumpSpeed;
                }
            }
        }
    }

    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private float _fallStartPositionY;

        private Vector2 _direction, _desiredVelocity, _velocity;
        private float _maxSpeedChange, _acceleration;

        public override void Enter(State previousState) {
            base.Enter(previousState);
            // TODO: Reset hitbox, change to idle animation.
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
            _fallStartPositionY = _playerMovementController.rbPositionY;
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.FallAnimationClip);
        }

        public override void Update() {
            if(_playerData.CurrentPlayerStats.HasDoubleJump) {
                if(_psm.EnterJumpState()) {
                    return;
                }
            }
            // check sliding wall

            // Cap Max Fall Speed
            _playerMovementController.setVelocity(new Vector2(_playerMovementController.rbVelocityX, Mathf.Min(_playerMovementController.rbVelocityY, _playerData.CurrentPlayerStats.MaxFallSpeed)));
            
            if(_playerMovementController.Grounded) {
                _psm.EnterDefaultState();
            }

            _psm.RunState.RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerData);
        }

        public override void FixedUpdate() {
             _handleHorizontalMovement();
        }

        private void _handleHorizontalMovement() {
            _playerMovementController.setVelocity(_psm.RunState.RunStateRunLogicHandler.GetVelocityX(), _playerMovementController.rbVelocityY);
        }
    }
}
