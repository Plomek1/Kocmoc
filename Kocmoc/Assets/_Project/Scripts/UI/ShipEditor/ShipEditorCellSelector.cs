using Kocmoc.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kocmoc.UI
{
    public class ShipEditorCellSelector : SelectorButton<ShipCellBlueprint>
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text label;
        public override void Init(ShipCellBlueprint value)
        {
            base.Init(value);
            name = "Select: " + value.cellName;
            label.text = value.cellName;
            icon.sprite = value.icon;
        }
    }
}
