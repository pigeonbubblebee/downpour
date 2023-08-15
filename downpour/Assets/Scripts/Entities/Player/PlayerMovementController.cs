
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public Direction HeadDirection { get; private set; }
        [field: SerializeField] public int FacingDirection { get; private set; }
        public int MovingDirection { get; private set; }
        [SerializeField] private bool _spriteFacingRight = true;
        public bool SpriteFacingRight { get { return _spriteFacingRight; } }

        private Rigidbody2D _rb;
        public float rbVelocityX { get { return _rb.velocity.x; } }
        public float rbVelocityY { get { return _rb.velocity.y; } }
        public Vector2 rbVelocity { get { return _rb.velocity; } }
        public float rbPositionX { get { return _rb.position.x; } }
        public float rbPositionY { get { return _rb.position.y; } }

        public float CoyoteCounter { get; private set; }
        public float JumpBufferCounter { get; private set; }
        public bool DesiredJump { get; private set; }
        public bool IsJumpReset { get; private set; }
        public bool UsedDoubleJump { get; private set; }

        private bool _grounded;
        public bool Grounded { get { return _checkGroundedRequest(); } }

        private PlayerData.ColliderBounds _colliderBoundsSource { get; set; }

        // public InputAction JumpAction => InputReader.Instance.InputActions.Gameplay.Jump;


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
            }

            FacingDirection = 1;
        }

        private void FixedUpdate() {
            _checkCollisions();

            _handleJumpData();
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
            if(DesiredJump && IsJumpReset) {
                IsJumpReset = false;
                // DesiredJump = false;
                JumpBufferCounter = _playerStatsController.CurrentPlayerStats.JumpBufferTime;
            } else if(JumpBufferCounter > 0) {
                JumpBufferCounter -= Time.deltaTime;
            } else if (!DesiredJump) {
                IsJumpReset = true;
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
            _spriteFacingRight=!_spriteFacingRight;
            FacingDirection *= -1;
            Vector3 scale = transform.localScale;
            // scale.x *= -1;
            transform.localScale = scale;

            if((_playerStateMachine.CurrentState as PlayerState).CanFlip)
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

        private void _checkCollisions() {
            _grounded = OverlapBoxOnGround(_colliderBoundsSource.feetRect);
        }

        public Collider2D OverlapBoxOnGround(Rect bounds) {
            Vector2 charPosition = transform.position;
            Vector2 boundsPosition = bounds.position * transform.localScale;

            return Physics2D.OverlapBox(charPosition + boundsPosition, bounds.size, 0, Layers.GroundLayer);
        }
    }
}