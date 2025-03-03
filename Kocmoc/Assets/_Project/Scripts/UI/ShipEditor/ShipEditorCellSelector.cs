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

        [Space(20)]
        [SerializeField] private Color buttonSelectedColor;
        [SerializeField] private Color iconSelectedColor;

        public override void Init(ShipCellBlueprint value)
        {
            base.Init(value);
            name = "Select: " + value.cellName;
            label.text = value.cellName;
            icon.sprite = value.icon;
        }

        public override void Select()
        {
            icon.color = iconSelectedColor;
            button.targetGraphic.color = buttonSelectedColor;
        }

        public override void Deselect()
        {
            icon.color = Color.white;
            button.targetGraphic.color = Color.white;
        }
    }
}
