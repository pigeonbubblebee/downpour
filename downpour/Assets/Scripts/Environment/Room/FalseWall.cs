using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class FalseWall : MonoBehaviour
    {
        [field: SerializeField] private GameObject _falseWall;
        [field: SerializeField] private GameObject _falseArea;
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Player")) {
                _falseWall.SetActive(false);
                _falseArea.SetActive(false);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if(other.CompareTag("Player")) {
                _falseWall.SetActive(true);
                _falseArea.SetActive(true);
            }
        }
    }
}
