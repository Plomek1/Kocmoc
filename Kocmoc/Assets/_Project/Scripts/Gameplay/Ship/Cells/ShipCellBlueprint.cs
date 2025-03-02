using System.Collections.Generic;
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
        public Rotation validRotations = Rotation.Up;
        public Rotation connectionSides;

        [Space(20)]
        public ModuleBlueprint[] modules;

        [HideInInspector] public HashSet<Vector2Int> connectionPoints = new();

        private void CalculateInternalValues() //HACK
        {
            connectionPoints = new();
            if (connectionSides.HasFlag(Rotation.Up))
            {
                for (int x = 0; x < size.x; x++)
                    connectionPoints.Add(new Vector2Int(x, size.y));
            }

            if (connectionSides.HasFlag(Rotation.Down))
            {
                for (int x = 0; x < size.x; x++)
                    connectionPoints.Add(new Vector2Int(x, -1));
            }

            if (connectionSides.HasFlag(Rotation.Right))
            {
                for (int y = 0; y < size.x; y++)
                    connectionPoints.Add(new Vector2Int(size.x, y));
            }

            if (connectionSides.HasFlag(Rotation.Left))
            {
                for (int y = 0; y < size.y; y++)
                    connectionPoints.Add(new Vector2Int(-1, y));
            }
        }

        private void Awake()
        {
            CalculateInternalValues();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CalculateInternalValues();
        }
#endif
    }

    [System.Serializable]
    public class ShipCellData
    {
        public ShipData ship { get; private set; }
        
        [field: SerializeField] 
        public ShipCellBlueprint blueprint { get; private set; }
        [field: SerializeField]
        public Vector2Int coordinates {  get; private set; }
        [field: SerializeField]
        public Rotation currentRotation { get; private set; } = Rotation.Up;

        public ShipCell prefab => blueprint.prefab;
        public string cellName => blueprint.cellName;
        public Sprite icon     => blueprint.icon;
        public Vector2Int size => blueprint.size;
        public Rotation validRotations => blueprint.validRotations;
        public float mass      => blueprint.mass;

        public HashSet<Vector2Int> connectionPoints {  get; private set; }
        public ModuleData[] modules {  get; private set; }
        public int health;
        
        public void SetShip(ShipData ship) => this.ship = ship;

        private void CalculateConnectionPoints()
        {
            connectionPoints = new(blueprint.connectionPoints.Count);
            foreach (Vector2Int point in blueprint.connectionPoints)
            {
                Vector2Int transformedPoint = coordinates + point.RightAngleRotate(currentRotation);
                connectionPoints.Add(transformedPoint);
            }
        }

        private void AddModules()
        {
            int modulesCount = blueprint.modules.Length;

            modules = new ModuleData[modulesCount];
            for (int i = 0; i < modulesCount; i++)
            {
                modules[i] = blueprint.modules[i].CreateDataClass(this);
                modules[i].Init();
            }
        }

        public void Move(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
            CalculateConnectionPoints();
        }

        public void Rotate(Rotation rotation)
        {
            currentRotation = rotation;
            CalculateConnectionPoints();
        }

        public void Init()
        {
            CalculateConnectionPoints();
            AddModules();
        }

        public ShipCellData(ShipCellBlueprint blueprint, Vector2Int coordinates, Rotation rotation)
        {
            this.blueprint = blueprint;
            this.coordinates = coordinates;
            this.currentRotation = rotation;
            Init();
        }
    }
}
