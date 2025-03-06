using UnityEngine;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kocmoc.Gameplay
{
    public class ShipExporter : MonoBehaviour
    {
#if UNITY_EDITOR
        private static string baseExportPath = "Assets/_Project/Assets/Ships/";
        public static void ExportToBlueprint(ShipData ship, string fileName, string folder = "")
        {
            ShipBlueprint blueprint = ScriptableObject.CreateInstance<ShipBlueprint>();
            blueprint.name = fileName;
            blueprint.shipName = ship.name;

            GridCell<ShipCellData>[] shipCells = ship.grid.GetCells().ToArray();
            blueprint.cells = new ShipCellData[shipCells.Length];
            
            for (int i = 0; i < shipCells.Length; i++)
                blueprint.cells[i] = shipCells[i].value;

            string directiory = baseExportPath + folder;
            if (directiory[directiory.Length - 1] != '/') 
                directiory += '/';
            
            if (!Directory.Exists(directiory))
                Directory.CreateDirectory(directiory);

            AssetDatabase.CreateAsset(blueprint, directiory + fileName + ".asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
