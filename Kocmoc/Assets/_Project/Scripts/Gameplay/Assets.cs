using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Assets : MonoBehaviour
    {
        private static Assets _instance;

        public static Assets Instance 
        { 
            get 
            { 
                if (_instance == null) 
                    _instance = Instantiate((GameObject)Resources.Load("Assets")).GetComponent<Assets>();
                return _instance;
            }
        }

        [Header("Prefabs")]
        [field: SerializeField] public Ship shipPrefab {  get; private set; }
    }
}
