using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Downpour.Scenes
{
    public class SceneLoader
    {
        public struct SceneTransitionData {
            public string spawnPoint;
            public GameData gameData;
            public SceneReference currentScene;
        }

        public struct SceneUnloadData {
            public GameData gameData;
            public SceneReference currentScene;
            public SceneReference nextScene;
        }

        public static event Action SceneLoadEvent;
        public static event Action BeforeSceneLoadEvent;
        public static readonly string MAIN_MENU = "MainMenu";

        public SceneReference activeScene { get; private set; }

        public static void LoadMainMenu() {
            LoadScene(MAIN_MENU);
        }

        public static void LoadScene(string sceneName) {
            SceneLoadEvent?.Invoke();
            SceneManager.LoadScene(sceneName);
        }

        public static void LoadScene(SceneReference sceneReference) {
            if(sceneReference != null) {
                SceneLoadEvent?.Invoke();
                SceneManager.LoadScene(sceneReference);
            }
        }

        private void OnUnloadScene(SceneReference nextScene) {
            SceneUnloadData unloadData = new SceneUnloadData() {
                gameData = DataManager.Instance.GameData,
                currentScene = activeScene,
                nextScene = nextScene,
            };
            
            // TODO: Fade to black

            BeforeSceneLoadEvent?.Invoke();
        }
    }
}
