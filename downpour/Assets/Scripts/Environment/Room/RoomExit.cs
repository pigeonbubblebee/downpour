using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    public class RoomExit : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneReference;

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Player")) {
                SceneLoader.LoadScene(sceneReference);
            }
        }
    }
}
