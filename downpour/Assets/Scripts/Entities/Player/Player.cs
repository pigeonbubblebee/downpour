using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(PlayerInteractableController))]
    [RequireComponent(typeof(PlayerAnimationController))]
    [RequireComponent(typeof(PlayerStatsController))]
    [RequireComponent(typeof(PlayerCombatController))]
    public class Player : Singleton<Player>
    {

        [field: SerializeField] public PlayerData PlayerData { get; private set; }
        public PlayerMovementController PlayerMovementController { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public PlayerInteractableController PlayerInteractableController { get; private set; }
        public PlayerAnimationController PlayerAnimationController { get; private set; }
        public PlayerStatsController PlayerStatsController { get; private set; }
        public PlayerCombatController PlayerCombatController { get; private set; }

        protected override void Awake() {
            base.Awake();
            PlayerMovementController = GetComponent<PlayerMovementController>();
            PlayerStateMachine = GetComponent<PlayerStateMachine>();
            PlayerInteractableController = GetComponent<PlayerInteractableController>();
            PlayerAnimationController = GetComponent<PlayerAnimationController>();
            PlayerStatsController = GetComponent<PlayerStatsController>();
            PlayerCombatController = GetComponent<PlayerCombatController>();
        }

        // Handles gizmos rendering in editor, if enabled
        #if UNITY_EDITOR
        private void OnDrawGizmos() {
            if(!PlayerData)
                return;

            if(!PlayerData.DrawGizmos)
                return;
            
            Transform t = transform;
            Vector2 position = (Vector2)t.position;
            Vector2 scale = (Vector2)t.localScale;

            GizmosDrawer drawer = new GizmosDrawer();
            
            DrawColliderData(PlayerData.StandColliderBounds);
            
            void DrawColliderData(PlayerData.ColliderBounds colliderBounds) {

                drawer.SetColor(GizmosColor.Player.colliderData)
                    .DrawWireSquare(position + (colliderBounds.bounds.min * scale), colliderBounds.bounds.size)
                    .SetColor(GizmosColor.Player.feet)
                    .DrawWireSquare(position + (colliderBounds.feetRect.min * scale), colliderBounds.feetRect.size)
                    .SetColor(GizmosColor.Player.hand)
                    .DrawWireSquare(position + (colliderBounds.handRightRect.min * scale), colliderBounds.handRightRect.size)
                    .SetColor(GizmosColor.Player.hand)
                    .DrawWireSquare(position + (colliderBounds.handLeftRect.min * scale), colliderBounds.handLeftRect.size)
                    .SetColor(GizmosColor.Player.attack)
                    .DrawWireSquare(position + (colliderBounds.slashRightRect.min * scale), colliderBounds.slashRightRect.size)
                    .SetColor(GizmosColor.Player.attack)
                    .DrawWireSquare(position + (colliderBounds.slashLeftRect.min * scale), colliderBounds.slashLeftRect.size);
            }
        }
        #endif
    }
}
