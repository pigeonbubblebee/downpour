using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.Combat
{
    public class HealthSystem : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealthPoints { get; private set; }
        [field: SerializeField] public bool HealthPointsAreTrueHitAmount { get; private set; }
        public int CurrentHealthPoints { get; private set; }

        // TODO: implement health changes
    }
}
