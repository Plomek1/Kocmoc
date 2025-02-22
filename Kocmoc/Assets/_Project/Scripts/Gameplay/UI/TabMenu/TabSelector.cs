using UnityEngine;
using UnityEngine.UI;

namespace Kocmoc
{
    [RequireComponent(typeof(Button))]
    public class TabSelector : MonoBehaviour
    {
        [SerializeField] private TabGroup group;
        [SerializeField] private Tab tab;

        private Button btn;

        private void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(OpenTab);
        
            group.TabOpened += OnTabOpened;
        }

        public void OpenTab()
        {
            group.OpenTab(tab);
        }

        private void OnTabOpened(Tab openedTab)
        {
            btn.interactable = tab != openedTab;
        }
    }
}
