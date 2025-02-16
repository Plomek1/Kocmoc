using System.Runtime.CompilerServices;
using UnityEngine;

namespace Kocmoc
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float minZoom;
        [SerializeField] private float maxZoom;
        [SerializeField] private float smoothness;

        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
        }

        private void Update()
        {
            
        }
    }
}
