using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kocmoc.UI
{
    public abstract class ListSelector<T> : MonoBehaviour
    {
        public Action<T> ValueSelected;
        public Action ValueDeselected;

        [SerializeField] protected T[] values;
        [Space(10)]
        
        [SerializeField] private SelectorButton<T> selectorPrefab;
        [SerializeField] private Transform listRoot;
        [SerializeField] private Scrollbar scrollbar;

        private Dictionary<T, SelectorButton<T>> selectors;
        private T selectedValue;

        private void Start() => OnStart();
        protected virtual void OnStart()
        {
            selectors = new Dictionary<T, SelectorButton<T>>(values.Length);
            foreach (T value in values)
                selectors.Add(value, SpawnSelector(value));
            if (scrollbar) scrollbar.value = 1;
        }

        protected virtual SelectorButton<T> SpawnSelector(T value)
        {
            SelectorButton<T> selector = Instantiate(selectorPrefab, listRoot);
            selector.Init(value);
            selector.Selected += OnSelectorClicked;
            return selector;
        }

        private void OnSelectorClicked(T value)
        {
            if (object.Equals(value, selectedValue)) Deselect();
            else Select(value);
        }

        protected virtual void Select(T value)
        {
            Deselect();
            selectedValue = value;
            selectors[selectedValue].Select();

            ValueSelected?.Invoke(selectedValue);
        }

        protected virtual void Deselect()
        {
            if (object.Equals(selectedValue, default)) return;
            selectors[selectedValue].Deselect();
            selectedValue = default;

            ValueDeselected?.Invoke();
        }

        private void OnDisable()
        {
            //Deselect();
        }
    }
}
