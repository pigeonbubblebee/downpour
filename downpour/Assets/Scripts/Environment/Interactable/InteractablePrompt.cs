using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Downpour
{
    public class InteractablePrompt : Singleton<InteractablePrompt>
    {
        [SerializeField] private TextMeshPro _interactableText;
        [SerializeField] private GameObject _interactablePrompt;

        public void DisplayInteractablePrompt(string interactableText, Vector2 position) {
            _interactablePrompt.SetActive(true);
            transform.position = position;
            _interactableText.text = interactableText;
        }

        public void DisableInteractivePrompt() {
            _interactablePrompt.SetActive(false);
        }
    }
}
