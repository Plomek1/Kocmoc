using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public class TabGroup : MonoBehaviour
    {
        public Action<Tab> TabOpened;
        public Action<Tab> TabClosed;

        [SerializeField] private Tab startingTab;

        private Tab activeTab;

        private void Start()
        {
            OpenTab(startingTab);
        }

        public void OpenTab(Tab tab)
        {
            if (!tab) return;

            CloseActiveTab();
            activeTab = tab;
            activeTab.Open();
            TabOpened?.Invoke(activeTab);
        }

        private void CloseActiveTab()
        {
            if (!activeTab) return;

            activeTab.Close();
            TabClosed?.Invoke(activeTab);
            activeTab = null;
        }
    }
}
