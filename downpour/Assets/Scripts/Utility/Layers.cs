using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public static class Layers
    {
        public static readonly int GroundLayer = LayerMask.GetMask("Ground");
        public static readonly int HittableLayer = LayerMask.GetMask("Hittable");
        public static readonly int PlayerLayer = LayerMask.GetMask("Player");
        public static readonly int CameraZoneLayer = LayerMask.GetMask("CameraZone");
    }
}
