using UnityEngine;
using UnityEngine.EventSystems;

namespace Kocmoc
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float startZoom;
        [SerializeField] private float minZoom;
        [SerializeField] private float maxZoom;
        [SerializeField] private float zoomSensitivity;
        [SerializeField] private float smoothness;

        private float targetZoom;

        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();

            cam.orthographicSize = startZoom;
            targetZoom = startZoom;
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                targetZoom = targetZoom - Input.mouseScrollDelta.y * zoomSensitivity;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, smoothness * Time.deltaTime);
            if (Mathf.Abs(cam.orthographicSize - targetZoom) < .01f) cam.orthographicSize = targetZoom;
        }
    }
}
