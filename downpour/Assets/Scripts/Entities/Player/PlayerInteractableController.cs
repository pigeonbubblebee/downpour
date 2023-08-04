using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerInteractableController : PlayerComponent
    {
        private InteractablePrompt _interactablePrompt;
        
        protected void Start() {
            _interactablePrompt = InteractablePrompt.Instance;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Interactable")) {
                if(other.transform.TryGetComponent(out IInteractable interactable)) {
                    _interactablePrompt.DisplayInteractablePrompt(interactable.InteractText(this.Player), 
                        new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 2f));
                    // interactable input
                    if(interactable.CanInteract(this.Player)) {
                        // interact
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if(other.CompareTag("Interactable")) {
                if(other.transform.TryGetComponent(out IInteractable interactable)) {
                    _interactablePrompt.DisableInteractivePrompt();
                }
            }
        }
    }
}
