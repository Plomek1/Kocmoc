using UnityEngine;
using Kocmoc.Gameplay;

namespace Kocmoc.UI
{
    public class ShipEditorCellList : ListSelector<ShipCellBlueprint>
    {
        [Header("Ship Editor")]
        [SerializeField] private ShipEditor editor;

        protected override void OnStart()
        {
            base.OnStart();
            editor.Closed += Deselect;
        }

        protected override void Select(ShipCellBlueprint value)
        {
            base.Select(value);
            editor.SelectBlueprint(value);
        }

        protected override void Deselect()
        {
            if (object.Equals(selectedValue, default)) return;
            base.Deselect();
            editor.DeselectBlueprint();
        }
    }
}
