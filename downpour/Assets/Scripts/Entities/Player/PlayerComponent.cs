using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(Player))]
    public abstract class PlayerComponent : MonoBehaviour
    {
        public Player Player { get; private set; }

        protected PlayerData _playerData => Player.PlayerData;

        protected PlayerMovementController _playerMovementController => Player.PlayerMovementController;
        protected PlayerStateMachine _playerStateMachine => Player.PlayerStateMachine;

        protected virtual void Awake() {
            Player = GetComponent<Player>();
        }
    }
}
