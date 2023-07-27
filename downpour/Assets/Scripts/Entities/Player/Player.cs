using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Player : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField] private bool _drawGizmos;
        #endif

        [field: SerializeField] public PlayerData PlayerData { get; private set; }
        public PlayerMovementController PlayerMovementController { get; private set; }
        public PlayerStateMachine PlayerStateMachine { get; private set; }

        private void Awake() {
            PlayerMovementController = GetComponent<PlayerMovementController>();
            PlayerStateMachine = GetComponent<PlayerStateMachine>();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos() {
            if(!PlayerData || !_drawGizmos)
                return;
            
            Transform t = transform;
            Vector2 position = (Vector2)t.position;
            Vector2 scale = (Vector2)t.localScale;

            GizmosDrawer drawer = new GizmosDrawer();
            
            DrawColliderData(PlayerData.StandColliderBounds);
            
            void DrawColliderData(PlayerData.ColliderBounds colliderBounds) {
                if (!colliderBounds.drawGizmos)
                    return;

                drawer.SetColor(GizmosColor.Player.colliderData)
                    .DrawWireSquare(position + (colliderBounds.bounds.min * scale), colliderBounds.bounds.size)
                    .SetColor(GizmosColor.Player.feet)
                    .DrawWireSquare(position + (colliderBounds.feetRect.min * scale), colliderBounds.feetRect.size)
                    .SetColor(GizmosColor.Player.hand)
                    .DrawWireSquare(position + (colliderBounds.handRect.min * scale), colliderBounds.handRect.size);
            }
        }
        #endif
    }
}
