using UnityEngine;

namespace Kocmoc
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }

        private void Awake()
        {
            if (instance)
                Destroy(instance.gameObject);
            instance = this as T;
        }
    }
}
