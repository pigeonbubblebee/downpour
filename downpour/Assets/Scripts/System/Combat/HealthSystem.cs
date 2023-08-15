using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Downpour.Combat
{
    public class HealthSystem : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealthPoints { get; private set; }
        [field: SerializeField] public bool HealthPointsAreTrueHitAmount { get; private set; }
        public int CurrentHealthPoints { get; private set; }

        public event Action DeathEvent;
        public event Action<int> DamageEvent;

        // TODO: implement health changes
        private void Awake() {
            CurrentHealthPoints = MaxHealthPoints;
        }

        public void SetMaxHealth(int health) {
            MaxHealthPoints = health;
        }

        public void ResetHealth() {
            CurrentHealthPoints = MaxHealthPoints;
        }

        public void AddHealth(int health, bool overflow) {
            CurrentHealthPoints += health;

            if(!overflow) {
                CurrentHealthPoints = Mathf.Min(CurrentHealthPoints, MaxHealthPoints);
            }
        }

        public void TakeDamage(int damage) {
            CurrentHealthPoints -= HealthPointsAreTrueHitAmount ? 1 : damage;
            DamageEvent.Invoke(damage);
            if(CurrentHealthPoints <= 0) {
                DeathEvent?.Invoke();
            }
        }
    }
}
