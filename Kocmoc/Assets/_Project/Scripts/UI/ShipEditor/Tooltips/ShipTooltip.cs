using Kocmoc.Gameplay;
using UnityEngine;
using TMPro;

namespace Kocmoc.UI
{
    public class ShipTooltip : Tooltip
    {
        [SerializeField] private ShipEditor editor;

        [SerializeField] private TMP_Text nameLabel;

        private void Awake()
        {
            editor.ShipChanged += ShowTooltip;
        }

        protected override void UpdateTooltip()
        {
            Debug.Log(target);
            Ship targetShip = target as Ship;
            if (!targetShip) return;

            nameLabel.text= targetShip.data.name;
        }
    }
}
