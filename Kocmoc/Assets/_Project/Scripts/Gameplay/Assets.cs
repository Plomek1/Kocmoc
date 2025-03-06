using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class Assets : MonoBehaviour
    {
        private static Assets p_instance;

        public static Assets Instance 
        { 
            get 
            { 
                if (p_instance == null) 
                    p_instance = Instantiate((GameObject)Resources.Load("Assets")).GetComponent<Assets>();
                return p_instance;
            }
        }

        private void Awake()
        {
            inputReader = ScriptableObject.CreateInstance<InputReader>();
        }

        [Header("Settings")]
        public InputReader inputReader {  get; private set; }

        [Header("Prefabs")]
        [field: SerializeField] public Ship shipPrefab {  get; private set; }
    }
}
