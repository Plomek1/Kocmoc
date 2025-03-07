using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kocmoc.UI
{
    public class DevConsole : Menu
    {
        [SerializeField] private TMP_InputField input;
        [SerializeField] private TMP_Text output;

        private void Start()
        {
            Globals.Instance.inputReader.UISubmit += SubmitCommand;
        }

        public void ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            output.text += command + "\n";
        }

        private void SubmitCommand()
        {
            ExecuteCommand(input.text);
            input.text = string.Empty;
            input.ActivateInputField();
        }

        public override void Open()
        {
            base.Open();
            gameObject.SetActive(true);
            Globals.Instance.inputReader.DisableShipInput();

            input.text = string.Empty;
            input.ActivateInputField();
        }

        public override void Close()
        {
            base.Close();
            gameObject.SetActive(false);
            Globals.Instance.inputReader.EnableShipInput();
        }
    }
}
