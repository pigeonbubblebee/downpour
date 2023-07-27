
using UnityEngine;

namespace Downpour {
    // Base Class For Using Singleton Instance.
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> { 
        private static T _instance;
        
        public static T Instance { 
            get {
                return _instance; 
            } 
        }

        protected virtual void Awake() {
            if(_instance != null && _instance != this as T) {
                Destroy(gameObject);
            } else {
                _instance = this as T;
            }
        }

        protected virtual void onDestroy() {
            if(_instance == (this as T)) {
                _instance = null;
            }
        }

        // Locates Instance Of Singleton Object
        public static void FindInstance() {
            _instance = FindObjectOfType<T>();

            if(_instance == null) {
                Debug.LogError("No instance of type " + typeof(T).Name + " found in scene.");
            }
        }
    }
    
    // Base Class For Using Persistent Singleton Instance.
    public abstract class SingletonPersistent<T> : Singleton<T> where T : SingletonPersistent<T> {
        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            name = $"[{typeof(T).Name}]";
        }
    }

}
