using UnityEngine;

namespace Scripts.Helper
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            _instance = this as T;
        }
    }
}