using System;
using UnityEngine;

namespace Kocmoc.UI
{
    public class TabGroup : MonoBehaviour
    {
        public Action<Tab> TabOpened;
        public Action<Tab> TabClosed;

        [SerializeField] private Tab defaultTab;
        [SerializeField] private bool resetTabOnClose;

        private Tab activeTab;

        private void Start()
        {
            OpenTab(defaultTab);
        }

        public void OpenDefaultTab() => OpenTab(defaultTab);

        public void OpenTab(Tab tab)
        {
            if (!tab || activeTab == tab) return;

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
