using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kocmoc
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] protected bool opened;
        
        [Header("Menu Events")]
        public UnityEvent MenuOpened;
        public UnityEvent MenuClosed;

        public virtual void Open()
        {
            if (opened) return;
            opened = true;
            MenuOpened?.Invoke();
        }

        public virtual void Close()
        {
            if (!opened) return;
            opened = false;
            MenuClosed?.Invoke();
        }

        public void Toggle()
        {
            if (opened) Close();
            else Open();
        }
    }
}
