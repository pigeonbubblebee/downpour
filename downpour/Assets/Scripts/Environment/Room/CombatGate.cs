using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class CombatGate : MonoBehaviour
    {
        [field: SerializeField] public GameObject[] EncounterObjects { get; private set; }
        [field: SerializeField] private GameObject _combatGate;
    }
}
