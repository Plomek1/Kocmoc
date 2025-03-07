using UnityEngine;

namespace Kocmoc
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            else
                Instance = this as T;
        }
    }
}
