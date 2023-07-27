using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour;
using Downpour.Input;

namespace Downpour {
    public class GameManager : SingletonPersistent<GameManager>
    {   
        // Initialization requiring other scripts.
        private void Start() {
            InputReader.Instance.EnableGameplayInput();
        }
    }
}
