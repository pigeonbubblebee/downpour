
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Downpour.Input;

namespace Downpour.Entity.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : PlayerComponent
    {
        private InputReader _inputReader;

        // Enum to keep track of player direction
        public enum Direction { 
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        // Vars to keep track of direction.
        // Head direction is the way the blade will swing.
        // Facing direction is the direction the player is actually facing
        // Moving direction is the direction the player is moving. Can be zero if the player is not moving.
        public Direction HeadDirection { get; private set; }
        public int FacingDirection { get; private set; }
        public int MovingDirection { get; private set; }

        // Variable to keep track of true direction of the sprite. Used for flipping/
        private bool _spriteFacingRight = true;
        public bool SpriteFacingRight { get { return _spriteFacingRight; } }

        // Rigidbody with getters
        private Rigidbody2D _rb;
        public float rbVelocityX { get { return _rb.velocity.x; } }
        public float rbVelocityY { get { return _rb.velocity.y; } }
        public Vector2 rbVelocity { get { return _rb.velocity; } }
        public float rbPositionX { get { return _rb.position.x; } }
        public float rbPositionY { get { return _rb.position.y; } }

        // Jump Counters. Keeps track of coyote time and jump buffers.
        [field: SerializeField] public float CoyoteCounter { get; private set; }
        [field: SerializeField] public float JumpBufferCounter { get; private set; }

        // Bool to keep track of if the input is desiring a jump.
        public bool DesiredJump { get; private set; }

        // Bool to keep track of if the player has released the jump key.
        private bool _isJumpReset;
        public bool UsedDoubleJump { get; private set; }

        // Grounded bool with a public getter leading to _checkGroundedRequest
        private bool _grounded;
        public bool Grounded { get { return _checkGroundedRequest(); } }

        private PlayerData.ColliderBounds _colliderBoundsSource;

        public bool DesiredDash { get; private set; }
        public float DashCooldownCounter { get; private set; }
        public bool CanDash { get; private set; }

        public float DashBufferCounter { get; private set; }
        private bool _dashBuffered;

        public event Action<float> DashEvent;
        public event Action FinishDashEvent;


        // Initialization
        protected override void Awake() {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Initialization requiring other scripts.
        private void Start() {
            _inputReader =  InputReader.Instance;

            if(this.enabled) {
                _inputReader.MovementEvent += _handleMovementInput;
                _inputReader.JumpEvent += _handleJumpInput;
                _inputReader.DashEvent += _handleDashInput;
            }

            FacingDirection = 1;
        }

        private void FixedUpdate() {
            _checkCollisions();

            _handleJumpData();
        }

        private void Update() {
            if(DashBufferCounter > 0) {
                DashBufferCounter -= Time.deltaTime;
            } else {
                _dashBuffered = false;
            }

            if(DashCooldownCounter > 0) {
                CanDash = false;
                DashCooldownCounter -= Time.deltaTime;
            } else {
                CanDash = true;
            }

            if(!((_playerStateMachine.CurrentState is PlayerSlashState) || (_playerStateMachine.CurrentState is PlayerParryState) || (_playerStateMachine.CurrentState is PlayerDashState)) && _dashBuffered) {
                DesiredDash = true;
            }
        }

        // <summary>
        // Sets velocity of Player character.
        // </summary>
        public void setVelocity(float x, float y) {
            _rb.velocity = new Vector2(x, y);
        }

        public void setVelocity(Vector2 velocity) {
            _rb.velocity = velocity;
        }

        // Jump Movement

        private void _handleJumpInput(bool startingJump) {
            DesiredJump = startingJump;
        }

        private void _handleJumpData() {
            // Check if grounded to handle coyote time. If not, coyote time ticks down.
            if(Grounded) {
                CoyoteCounter = _playerStatsController.CurrentPlayerStats.CoyoteTime;
                UsedDoubleJump = false;
            } else {
                CoyoteCounter -= Time.deltaTime;
            }

            // Check for jumping while actively jumping to handle jump buffering.
            // Is Jump Reset checks for if the player has let go of the jump key and is ready to press it again to trigger a jump (in this case buffered)
            if(DesiredJump && _isJumpReset) {
                _isJumpReset = false;

                JumpBufferCounter = _playerStatsController.CurrentPlayerStats.JumpBufferTime;
            } else if(JumpBufferCounter > 0) {
                JumpBufferCounter -= Time.deltaTime;
            } else if (!DesiredJump) {
                _isJumpReset = true;
            }
        }

        public void UseDoubleJump() {
            UsedDoubleJump = true;
        }

        public void ResetJumpBuffer() {
            JumpBufferCounter = 0;
        }

        public void ResetCoyoteTime() {
            CoyoteCounter = 0;
        }

        // Horizontal Movement

        // Handles move event.
        private void _handleMovementInput(float x, float y, bool startingMovement) {
            _flipCheck();

            if(x != 0) {
                FacingDirection = x > 0 ? 1 : -1;

                if(y == 0) {
                    HeadDirection = x > 0 ? Direction.RIGHT : Direction.LEFT;
                }

                MovingDirection = startingMovement ? FacingDirection : 0;
            } else {
                MovingDirection = 0;
            }

            if(y != 0) {
                HeadDirection = y > 0 ? Direction.UP : Direction.DOWN;
            }
        }

        // <summary>
        // Flips sprite if needed.
        // <summary/>
        private void _flipCheck() {
            if(FacingDirection>0 && !_spriteFacingRight) {
                _flip();
            } else if(FacingDirection < 0 && _spriteFacingRight) {
                _flip();
            }
        }

        // <summary>
        // Flips sprite.
        // <summary/>
        private void _flip() {
            if(!(_playerStateMachine.CurrentState as PlayerState).CanFlip) {
                return;
            }
            _spriteFacingRight=!_spriteFacingRight;
            FacingDirection *= -1;

            
            _playerStateMachine.PlayStateAnimation();
        }

        // Colliders

        private bool _checkGroundedRequest() {
            _checkCollisions();
            return _grounded;
        }

        public void SetColliderBounds(PlayerData.ColliderBounds colliderBounds) {
            _colliderBoundsSource = colliderBounds;
            // _collider.offset = colliderBounds.bounds.min;
            // _collider.size = colliderBounds.bounds.size;
            _checkCollisions();
        }

        public PlayerData.ColliderBounds GetColliderBounds() {
            return _colliderBoundsSource;
        }

        private void _checkCollisions() {
            Vector2 charPosition = transform.position;
            Vector2 boundsPosition = _colliderBoundsSource.feetRect.position * transform.localScale;
            _grounded = Physics2D.OverlapBox(charPosition + boundsPosition, _colliderBoundsSource.feetRect.size, 0, Layers.GroundLayer);
        }

        private void _handleDashInput(bool startingDash) {
            if((_playerStateMachine.CurrentState is PlayerSlashState || _playerStateMachine.CurrentState is PlayerParryState || _playerStateMachine.CurrentState is PlayerDashState) &&  !_dashBuffered && startingDash) {
                _dashBuffered = true;
                DashBufferCounter = _playerStatsController.CurrentPlayerStats.DashBufferTime;
            }
            DesiredDash = startingDash;
        }

        public IEnumerator Dash() {
            DashEvent?.Invoke(_playerStatsController.CurrentPlayerStats.DashSpeed);
            
            _dashBuffered = false;

            DashCooldownCounter = _playerStatsController.CurrentPlayerStats.DashCooldown;
            CanDash = false;

            _playerStatsController.StartInvincibilityFrames(_playerStatsController.CurrentPlayerStats.DashLength);
            
            yield return new WaitForSeconds(_playerStatsController.CurrentPlayerStats.DashLength);

            FinishDashEvent?.Invoke();

            DesiredDash = false;
        }
    }
}