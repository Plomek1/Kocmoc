using Kocmoc.Gameplay;
using UnityEngine;
using TMPro;
using System.Text;

namespace Kocmoc.UI
{
    public class ShipTooltip : Tooltip
    {
        [SerializeField] private ShipEditor editor;

        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text contentLabel;

        private void Awake()
        {
            editor.ShipChanged += ShowTooltip;
        }

        protected override void UpdateTooltip()
        {
            Ship targetShip = target as Ship;
            if (!targetShip) return;

            nameLabel.text= targetShip.data.name;

            StringBuilder builder = new StringBuilder();
            foreach (TooltipField field in target.GetTooltipFields())
                builder.Append(field.GetString());
            contentLabel.text= builder.ToString();
        }
    }
}
