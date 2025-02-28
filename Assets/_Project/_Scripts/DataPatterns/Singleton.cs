using UnityEngine;

namespace Project.Singleton
{
    /// <summary>
    ///     This transforms the static instance into a basic singleton.
    ///     This will destroy any new versions created, leaving the original instance intact.
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogWarning("Instance of " + typeof(T).Name + " is null");
                }
            
                return _instance;
            }
        
            private set => _instance = value;
        }

        protected static T _instance;
        
        protected void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
        }
    }
}