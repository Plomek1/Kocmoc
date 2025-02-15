using Kocmoc.Gameplay;
using UnityEngine;

namespace Kocmoc
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

        [Header("Prefabs")]
        public Ship shipPrefab;
    }
}
