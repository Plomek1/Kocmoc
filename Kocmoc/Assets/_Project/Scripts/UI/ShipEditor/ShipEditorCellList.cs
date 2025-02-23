using UnityEngine;
using Kocmoc.Gameplay;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Kocmoc.UI
{
    public class ShipEditorCellList : ListSelector<ShipCellBlueprint>
    {
        [SerializeField] private ShipEditor editor;
    }
}
