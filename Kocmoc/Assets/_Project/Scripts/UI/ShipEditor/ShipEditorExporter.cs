using UnityEngine;
using Kocmoc.Gameplay;
using TMPro;

namespace Kocmoc.UI
{
    public class ShipEditorExporter : ShipEditor
    {
        [SerializeField] private ShipBlueprint defaultShip;

        [SerializeField] private TMP_InputField fileNameInput;
        [SerializeField] private TMP_InputField directoryInput;

        private void Start()
        {
            Ship ship = ShipSpawner.SpawnShip(defaultShip, ShipType.Wreck, Vector2.zero);
            SetShip(ship);
            Open();
        }

#if UNITY_EDITOR
        public void Save()
        {
            if (string.IsNullOrEmpty(fileNameInput.text))
            {
                Debug.LogWarning("File name can't be null!");
                return;
            }

            ShipExporter.ExportToBlueprint(ship.data, fileNameInput.text, directoryInput.text);
            Clear();
        }

        public void Clear()
        {
            Destroy(this.ship.gameObject);
            Ship ship = ShipSpawner.SpawnShip(defaultShip, ShipType.Wreck, Vector2.zero);
            SetShip(ship);
            Open();
        }
#endif
    }
}
