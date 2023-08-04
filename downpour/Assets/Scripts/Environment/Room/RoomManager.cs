using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class RoomManager : Singleton<RoomManager>
    {
        [field: SerializeField] public int RoomNumber { get; private set; }
        [field: SerializeField] public string AreaName { get; private set; }

        // TODO: add room serialization
    }
}
