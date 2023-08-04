using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class DataManager : SingletonPersistent<DataManager>
    {
        private GameData _gameData;

        public GameData GameData {
            get {
                if(_gameData != null)
                    return _gameData; // TODO: add editor temp gamedata
                else
                    return null;
            }
        }
    }
}
