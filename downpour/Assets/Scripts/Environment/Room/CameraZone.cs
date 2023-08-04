using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class CameraZone : MonoBehaviour
    {
        private PolygonCollider2D _polygonCollider2D;

        private void Awake() {
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("PlayerCameraZoneChanger")) {
                CameraManager.Instance.SetVCameraConfines(_polygonCollider2D);
            }
        }
    }
}
