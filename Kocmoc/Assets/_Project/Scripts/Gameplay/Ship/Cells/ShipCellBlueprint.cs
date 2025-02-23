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
        public float mass;

        [Space(20)]
        public ModuleBlueprint[] modules;
    }

    [System.Serializable]
    public class ShipCellData
    {
        public ShipData ship { get; private set; }

        [SerializeField] private ShipCellBlueprint blueprint;

        public ShipCell prefab => blueprint.prefab;
        public float mass => blueprint.mass;
        public ModuleData[] modules;

        public Vector2Int coordinates;
        public Rotation currentRotation;
        public int health;

        public ShipCellData(ShipCellBlueprint blueprint, ShipData ship)
        {
            this.blueprint = blueprint;
            Init(ship);
        }

        public void Init(ShipData ship)
        {
            this.ship = ship;
            int modulesCount = blueprint.modules.Length;

            modules = new ModuleData[modulesCount];
            for (int i = 0; i < modulesCount; i++)
            {
                modules[i] = blueprint.modules[i].CreateDataClass(this);
                modules[i].Init();
            }
        }
    }
}
