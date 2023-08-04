using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Downpour.Entity.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/Entity/Player Data")]
    public class PlayerData : ScriptableObject
    {
        #if UNITY_EDITOR
        public bool DrawGizmos;
        #endif
        [Serializable]
        public class ColliderBounds {
            public Rect bounds;
            public Rect feetRect;
            public Rect handRect;
        }

        [field: SerializeField] public ColliderBounds StandColliderBounds { get; private set; }

        [field: SerializeField] public PlayerStats BasePlayerStats { get; private set; }
        public PlayerStats CurrentPlayerStats => BasePlayerStats;

        [Serializable]
        public struct PlayerStats {
            [field: SerializeField, Range(0f, 100f)] public float MoveSpeed { get; private set; }
            [field: SerializeField, Range(0f, 100f)] public float MaxAcceleration { get; private set; }

            [field: SerializeField, Range(0f, 10f)] public float JumpHeight { get; private set; }
            [field: SerializeField, Range(0f, 0.3f)] public float CoyoteTime { get; private set; }
            [field: SerializeField, Range(0f, 0.3f)] public float JumpBufferTime { get; private set; }

            [field: SerializeField] public bool HasDoubleJump;
        }
    }
}
