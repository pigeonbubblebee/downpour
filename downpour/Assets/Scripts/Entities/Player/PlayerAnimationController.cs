using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Downpour.Entity.Player
{
    public class PlayerAnimationController : PlayerComponent
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }

        [field: SerializeField] public AsymmetricalAnimationClip IdleAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip RunAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip JumpAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip FallAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip SlashAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip Slash2AnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip ParryAnimationClip { get; private set; }
        [field: SerializeField] public AsymmetricalAnimationClip DashAnimationClip { get; private set; }

        public string CurrentAnimation { get; private set; }

        public void PlayAnimation(AsymmetricalAnimationClip animationClip) {
            _resetAnimationSpeed(); // Reset Animation Speed
            animationClip.PlayAnimation(PlayerAnimator, _playerMovementController.SpriteFacingRight);
        }

        public void PlayAnimation(AsymmetricalAnimationClip animationClip, float speed) {
            _setAnimationSpeed(speed); // Reset Animation Speed
            animationClip.PlayAnimation(PlayerAnimator, _playerMovementController.SpriteFacingRight);
        }

        private void _resetAnimationSpeed() {
            _setAnimationSpeed(1f);
        }

        private void _setAnimationSpeed(float speed) {
            PlayerAnimator.speed = 1/speed;
        }

        [Serializable]
        public struct AsymmetricalAnimationClip {
            [field: SerializeField] public string RightClip { get; private set; }
            [field: SerializeField] public string LeftClip { get; private set; }

            public void PlayAnimation(Animator animator, bool facingRight) {
                animator.Play(facingRight ? RightClip : LeftClip);
            }
        }
    }
}
