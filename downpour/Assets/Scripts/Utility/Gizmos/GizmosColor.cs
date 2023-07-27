#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public static class GizmosColor
    {
        public class PlayerColors {
            public Color feet = Color.green;
            public Color hand = Color.yellow;
            public Color attack = Color.cyan;
            public Color colliderData = Color.green;
        }

        public static PlayerColors Player = new PlayerColors();
    }
}
#endif