using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private ShipCellData[] cells;

        void Start()
        {
            ShipSpawner.SpawnShip(cells, Vector2.zero);
        }
    }
}
