using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour
{
    public interface IInteractable
    {
        void OnInteract(Player player);
        bool CanInteract(Player player); // TODO add save data
        string InteractText(Player player); // TODO add save data
    }
}
