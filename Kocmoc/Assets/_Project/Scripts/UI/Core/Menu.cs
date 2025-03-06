using UnityEngine;
using UnityEngine.Events;
using Kocmoc.Gameplay;

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
            Assets.Instance.inputReader.UIBackCallbacks.Push(Close);

            MenuOpened?.Invoke();
        }

        public virtual void Close()
        {
            if (!opened) return;
            opened = false;

            //If the menu was closed without pressing esc we need to pop the callback
            if (Assets.Instance.inputReader.UIBackCallbacks.Peek() == Close)
                Assets.Instance.inputReader.UIBackCallbacks.Pop();

            MenuClosed?.Invoke();
        }

        public void Toggle()
        {
            if (opened) Close();
            else Open();
        }
    }
}
