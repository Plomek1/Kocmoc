using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kocmoc.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class SelectorButton<T> : MonoBehaviour
    {
        public Action<T> Selected;

        private T value;

        private Button button;

        public virtual void Init(T value)
        {
            this.value = value;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            Selected?.Invoke(value);
        }

        public void Select()
        {
            button.interactable = false;
        }

        public void Deselect()
        {
            button.interactable = true;

        }
    }
}
