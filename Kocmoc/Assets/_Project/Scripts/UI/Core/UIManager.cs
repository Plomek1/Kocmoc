using UnityEngine;

namespace Kocmoc.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DevConsole devConsole;
        [SerializeField] private ShipEditor shipEditor;

        private void Start()
        {
            Globals.Instance.inputReader.UIOpenDevConsole   += devConsole.Toggle;
            Globals.Instance.inputReader.UIOpenBuildingMenu += shipEditor.Toggle;
        }
    }
}
