using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kocmoc.UI
{
    public abstract class SelectorButton<T> : MonoBehaviour
    {
        public Action<T> Selected;

        private T value;

        [SerializeField] protected Button button;

        public virtual void Init(T value)
        {
            this.value = value;

            if (!button)
            {
                if (TryGetComponent(out Button button))
                    button = GetComponent<Button>();
                else
                {
                    Debug.LogWarning("Button selector doesn't have button component assigned!");
                    return;
                }
            }

            button.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            Selected?.Invoke(value);
        }

        public virtual void Select()
        {
            button.interactable = false;
        }

        public virtual void Deselect()
        {
            button.interactable = true;

        }
    }
}
