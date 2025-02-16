using UnityEngine;

namespace Kocmoc.Gameplay
{
    [CreateAssetMenu(fileName = "Cell", menuName = "Ship/Cell Blueprint", order = 0)]
    public class ShipCellBlueprint : ScriptableObject
    {
        public GameObject prefab;

    }

    [System.Serializable]
    public class ShipCellData
    {
        [SerializeField] private ShipCellBlueprint blueprint;

        public Vector2Int coordinates;
        public int health;

        public GameObject prefab => blueprint.prefab;

        public ShipCellData(ShipCellBlueprint blueprint)
        {
            this.blueprint = blueprint;
        }
    }
}
