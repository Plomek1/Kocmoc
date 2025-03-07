using UnityEngine;

namespace Kocmoc
{
    public class Globals : MonoBehaviour
    {
        private static Globals _instance;

        public static Globals Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Instantiate((GameObject)Resources.Load("Globals")).GetComponent<Globals>();
                return _instance;
            }
        }

        private void Awake()
        {
            inputReader.EnablePlayerActions();
            inputReader.ResetCallbacks();
        }

        [Header("Prefabs")]
        [field: SerializeField] public InputReader inputReader { get; private set; }
    }
}
