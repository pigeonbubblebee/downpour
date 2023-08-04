using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
