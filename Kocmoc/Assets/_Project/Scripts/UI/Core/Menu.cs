using UnityEngine;
using UnityEngine.Events;
using Kocmoc.Gameplay;

namespace Kocmoc
{
    public class Menu : MonoBehaviour
    {
        [field: SerializeField] public bool opened {  get; private set; }
        [SerializeField] private bool closeWithEscape = true;

        [Header("Menu Events")]
        public UnityEvent MenuOpened;
        public UnityEvent MenuClosed;

        public virtual void Open()
        {
            if (opened) return;
            opened = true;

            if (closeWithEscape)
                Globals.Instance.inputReader.UIBackCallbacks.Add(Close);
            MenuOpened?.Invoke();
        }

        public virtual void Close()
        {
            if (!opened) return;
            opened = false;

            //If the menu was closed without pressing esc we need to pop the callback
            if (closeWithEscape)
                Globals.Instance.inputReader.UIBackCallbacks.Remove(Close);
            MenuClosed?.Invoke();
        }

        public void Toggle()
        {
            if (opened) Close();
            else Open();
        }
    }
}
