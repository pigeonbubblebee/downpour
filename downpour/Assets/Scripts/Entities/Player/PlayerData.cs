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
            public Rect slashRightRect;
            public Rect slashLeftRect;
            public Rect handRightRect;
            public Rect handLeftRect;
        }

        [field: SerializeField] public ColliderBounds StandColliderBounds { get; private set; }

        [field: SerializeField] public PlayerStats BasePlayerStats { get; private set; }

        [Serializable]
        public struct PlayerStats {
            [field: SerializeField] public int MaxHealth { get; private set; }
            [field: SerializeField] public int MaxSpirit { get; private set; }

            [field: SerializeField, Range(0f, 100f)] public float MoveSpeed { get; private set; }

            [field: SerializeField, Range(0f, 10f)] public float JumpHeight { get; private set; }
            [field: SerializeField, Range(0f, 100f)] public float MaxFallSpeed { get; private set; }
            [field: SerializeField, Range(0f, 1f)] public float CoyoteTime { get; private set; }
            [field: SerializeField, Range(0f, 0.5f)] public float JumpBufferTime { get; private set; }
            [field: SerializeField] public bool HasDoubleJump;

            public int SlashDamage => BaseSlashDamageValues[SlashLevel];
            [field: SerializeField, Range(0, 4)] public int SlashLevel { get; private set; }
            [field: SerializeField] public int[] BaseSlashDamageValues { get; private set; }
            [field: SerializeField] public float SlashSpeed { get; private set; }
            [field: SerializeField] public float SlashCooldown { get; private set; }
            [field: SerializeField] public float SlashRange { get; private set; }
            [field: SerializeField] public float ComboTime { get; private set; }
            [field: SerializeField] public float SlashBufferTime { get; private set; }

            [field: SerializeField] public float SlashKnockbackMultiplier { get; private set; }
            [field: SerializeField] public float SlashKnockbackTime { get; private set; }
        }
    }
}
