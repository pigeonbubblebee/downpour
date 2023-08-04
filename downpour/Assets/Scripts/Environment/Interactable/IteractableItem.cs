using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour
{
    public class IteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _itemId;
        public void OnInteract(Player player) {
            Debug.Log(_itemId);
        }

        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "INSPECT";
        }
    }
}
