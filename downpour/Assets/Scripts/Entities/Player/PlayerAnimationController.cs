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

        public string CurrentAnimation { get; private set; }

        public void PlayAnimation(AsymmetricalAnimationClip animationClip) {
            animationClip.PlayAnimation(PlayerAnimator, _playerMovementController.SpriteFacingRight);
        }

        [Serializable]
        public struct AsymmetricalAnimationClip {
            [field: SerializeField] public string RightClip { get; private set; }
            [field: SerializeField] public string LeftClip { get; private set; }

            public void PlayAnimation(Animator animator, bool facingRight) {
                Debug.Log(facingRight ? RightClip : LeftClip);
                animator.Play(facingRight ? RightClip : LeftClip);
            }
        }
    }
}
