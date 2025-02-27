using System.Drawing;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "Cell", menuName = "Ship/Cell Blueprint", order = 0)]
    public class ShipCellBlueprint : ScriptableObject
    {
        public ShipCell prefab;
        [Space(10)]

        [Header("General")]
        public string cellName;
        public Sprite icon;
        [Space(10)]
        public Vector2Int size = Vector2Int.one;
        public float mass = 1;
        [Space(10)]
        public Rotation possibleRotations = Rotation.Up;
        public Rotation connectionSides;

        [Space(20)]
        public ModuleBlueprint[] modules;
    }

    [System.Serializable]
    public class ShipCellData
    {
        public ShipData ship { get; private set; }

        [SerializeField] private ShipCellBlueprint blueprint;

        public ShipCell prefab => blueprint.prefab;
        public string cellName => blueprint.cellName;
        public Sprite icon     => blueprint.icon;
        public Vector2Int size => blueprint.size;
        public float mass      => blueprint.mass;

        public ModuleData[] modules;

        public Vector2Int coordinates;
        public Rotation currentRotation;
        public int health;

        public ShipCellData(ShipCellBlueprint blueprint, Vector2Int coordinates, Rotation rotation)
        {
            this.blueprint = blueprint;
            this.coordinates = coordinates;
            this.currentRotation = rotation;
            Init();
        }

        public void Init()
        {
            int modulesCount = blueprint.modules.Length;

            modules = new ModuleData[modulesCount];
            for (int i = 0; i < modulesCount; i++)
            {
                modules[i] = blueprint.modules[i].CreateDataClass(this);
                modules[i].Init();
            }
        }

        public void SetShip(ShipData ship) => this.ship = ship;
    }
}
