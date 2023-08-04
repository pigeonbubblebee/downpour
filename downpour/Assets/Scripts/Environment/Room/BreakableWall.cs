using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Combat;
using Downpour.Entity.Player;

namespace Downpour
{
    [RequireComponent(typeof(HealthSystem))]
    public class BreakableWall : MonoBehaviour, IHittable
    {
        [field: SerializeField] private GameObject _breakableWall;
        [field: SerializeField] private GameObject _falseArea;
        private HealthSystem _healthSystem;

        private void Awake() {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void OnHit(Player player, int damage) {
            return; // TODO: implement taking damage
        }
    }
}
