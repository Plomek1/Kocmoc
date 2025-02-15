using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Kocmoc

{
    [CreateAssetMenu(fileName = "Cell", menuName = "Ship/Cell", order = 0)]
    public class ShipCellData : ScriptableObject
    {
        public GameObject prefab;

        [HideInInspector] public Vector2Int coordinates;
        [HideInInspector] public int health;
    }
}
