using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    public class GameInitialization : MonoBehaviour
    {
        private void Start() {
            SceneLoader.LoadMainMenu();
        }
    }
}
