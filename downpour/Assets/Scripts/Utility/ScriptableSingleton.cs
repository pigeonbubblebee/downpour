using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
    {
         private static T s_instance;

        /// <summary>
        /// The instance of type(<see cref="T"/>) located in the resources,
        /// if it does not exist in the project, creates a new one
        /// </summary>
        public static T Instance {
            get {
                if (s_instance)
                    return s_instance;

                return s_instance = ScriptableObject.CreateInstance<T>();
            }
        }
    }
}
