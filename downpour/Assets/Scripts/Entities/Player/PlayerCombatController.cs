using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Input;

namespace Downpour.Entity.Player
{
    public class PlayerCombatController : PlayerComponent
    {
        private InputReader _inputReader;
        public bool DesiredSlash { get; private set; }

        public event Action<int, float, float> SlashEvent;
        public event Action FinishSlashEvent;
        private void Start() {
            _inputReader =  InputReader.Instance;

            if(this.enabled) {
                _inputReader.SlashEvent += _handleSlashInput;
            }
        }

        private void _handleSlashInput(bool startingSlash) {
            DesiredSlash = startingSlash;
        }

        public IEnumerator Slash() {
            SlashEvent?.Invoke(_playerStatsController.CurrentPlayerStats.SlashDamage, _playerStatsController.CurrentPlayerStats.SlashSpeed, _playerStatsController.CurrentPlayerStats.SlashRange);
            
            Debug.Log("Slash");
            yield return new WaitForSeconds(_playerStatsController.CurrentPlayerStats.SlashSpeed);
            Debug.Log("Finished Slash Wait");

            FinishSlashEvent?.Invoke();
        }
    }
}
