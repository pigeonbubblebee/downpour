using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour;
using Downpour.Input;

namespace Downpour {
    public class GameManager : SingletonPersistent<GameManager>
    {   
        public enum GameState {
            Menu,
            Gameplay
        }

        public GameState CurrentGameState { get; private set; }

        protected override void Awake() { // temp
            base.Awake();
            CurrentGameState = GameState.Gameplay;
        }

        // Initialization requiring other scripts.
        private void Start() {
            InputReader.Instance.EnableGameplayInput();

            // Physics2D.IgnoreLayerCollision(8, 9);
        }
    }
}
