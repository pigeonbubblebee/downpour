using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Downpour
{
    public class CameraShaker : MonoBehaviour
    {
        private CinemachineVirtualCamera _vcam;
        private float _shakeTimer;
        private void Start() {
            _vcam = CameraManager.Instance.VCamera;
        }

        public void Shake(float duration, float magnitude) {
            CinemachineBasicMultiChannelPerlin noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            noise.m_AmplitudeGain = magnitude;
            _shakeTimer = duration;
        }

        private void Update() {
            if(_shakeTimer > 0) {
                _shakeTimer -= Time.deltaTime;
                if(_shakeTimer <= 0f) {
                    CinemachineBasicMultiChannelPerlin noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.m_AmplitudeGain = 0f;
                }
            }
        }
    }
}
