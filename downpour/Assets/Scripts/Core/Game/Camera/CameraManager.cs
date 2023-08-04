using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Downpour.Entity.Player;

namespace Downpour
{
    public class CameraManager : SingletonPersistent<CameraManager>
    {
        public Camera MainCamera => Camera.main;
        public CinemachineVirtualCamera VCamera { get; private set; }
        public CinemachineConfiner2D VCameraConfines { get; private set; }

        protected override void Awake() {
            base.Awake();
            VCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            VCameraConfines = GetComponentInChildren<CinemachineConfiner2D>();
        }

        protected void Start() {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay)
                VCamera.m_Follow = Player.Instance.transform;
        }

        public void SetVCameraConfines(PolygonCollider2D collider2D) {
            VCameraConfines.m_BoundingShape2D = collider2D;
        }
    }
}
