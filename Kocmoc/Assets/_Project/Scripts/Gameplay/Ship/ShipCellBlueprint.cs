using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "Cell", menuName = "Ship/Cell Blueprint", order = 0)]
    public class ShipCellBlueprint : ScriptableObject
    {
        public ShipCell prefab;
        public float mass;

        [Space(20)]
        public ModuleBlueprint[] modules;
    }

    [System.Serializable]
    public class ShipCellData
    {
        [SerializeField] private ShipCellBlueprint blueprint;

        public ShipCell prefab => blueprint.prefab;
        public float mass => blueprint.mass;
        public ModuleData[] modules;

        public Vector2Int coordinates;
        public int health;


        public ShipCellData(ShipCellBlueprint blueprint)
        {
            this.blueprint = blueprint;
            Init();
        }

        public void Init()
        {
            int modulesCount = blueprint.modules.Length;

            modules = new ModuleData[modulesCount];
            for (int i = 0; i < modulesCount; i++)
                modules[i] = blueprint.modules[i].CreateDataClass();
        }
    }
}
