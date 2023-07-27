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
        
        public PlayerState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }
    }

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter(State previousState) {
            _playerMovementController.setVelocity(0, _playerMovementController.rbVelocityY);
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
            // TODO: change to idle animation.
        }

        public override void Update() {
            // TODO: Check for other states
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
        public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private Vector2 _direction, _desiredVelocity, _velocity;
        private float _maxSpeedChange, _acceleration;

        public override void Enter(State previousState) {
            // TODO: Reset hitbox, change to idle animation.
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void Update() {
            if(_psm.EnterJumpState()
            || _psm.EnterFallState()) {
                return;
            }
            if(_playerMovementController.MovingDirection == 0) {
                _psm.ChangeState(_psm.IdleState);
            } else {
                _direction.x = _playerMovementController.MovingDirection;
                _desiredVelocity = new Vector2(_direction.x * _playerData.CurrentPlayerStats.MoveSpeed, 0f);
            }
        }

        public override void FixedUpdate() {
            _velocity = _playerMovementController.rbVelocity;
            _acceleration = _playerData.CurrentPlayerStats.MaxAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _playerMovementController.setVelocity(_velocity);
        }
    }

    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private Vector2 _direction, _desiredHorzVelocity, _velocity;
        private float _maxSpeedChange, _acceleration, _jumpSpeed;
        private bool _isJumping;

        public override void Enter(State previousState) {
            // TODO: Reset hitbox, change to idle animation.
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void Update() {
            _direction.x = _playerMovementController.MovingDirection;
            _desiredHorzVelocity = new Vector2(_direction.x * _playerData.CurrentPlayerStats.MoveSpeed, 0f);
        }

        public override void FixedUpdate() {
            _handleHorizontalMovement();
            _handleJump();

            _playerMovementController.setVelocity(_velocity);

            if(_psm.EnterFallState()) {
                return;
            }
        }

        private void _handleHorizontalMovement() {
            _velocity = _playerMovementController.rbVelocity;
            _acceleration = _playerData.CurrentPlayerStats.MaxAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredHorzVelocity.x, _maxSpeedChange);
        }

        private void _handleJump() {
            // Debug.Log(_playerMovementController.Grounded + " " + _playerMovementController.rbVelocityY);
            if(_playerMovementController.Grounded && _playerMovementController.rbVelocityY == 0) {
                _isJumping = false;
            }

            if(_playerMovementController.DesiredJump) {
                if(_playerMovementController.JumpBufferCounter > 0) {
                    _jumpAction();
                }
            } else {
                _velocity = new Vector2(_playerMovementController.rbVelocityX, 0.15f);
                _psm.EnterDefaultState();
            }
        }

        private void _jumpAction() {
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

    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private float _fallStartPositionY;

        private Vector2 _direction, _desiredVelocity, _velocity;
        private float _maxSpeedChange, _acceleration;

        public override void Enter(State previousState) {
            // TODO: Reset hitbox, change to idle animation.
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
            _fallStartPositionY = _playerMovementController.rbPositionY;
        }

        public override void Update() {
            if(_playerData.CurrentPlayerStats.HasDoubleJump) {
                if(_psm.EnterJumpState()) {
                    return;
                }
            }
            // check sliding wall
            if(_playerMovementController.Grounded) {
                _psm.EnterDefaultState();
            }

            _direction.x = _playerMovementController.MovingDirection;
            _desiredVelocity = new Vector2(_direction.x * _playerData.CurrentPlayerStats.MoveSpeed, 0f);
        }

        public override void FixedUpdate() {
             _handleHorizontalMovement();
        }

        private void _handleHorizontalMovement() {
            _velocity = _playerMovementController.rbVelocity;

            _acceleration = _playerData.CurrentPlayerStats.MaxAcceleration;

            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _playerMovementController.setVelocity(_velocity);
        }
    }
}
