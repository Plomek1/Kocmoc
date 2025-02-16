using TMPro;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class CameraDrag : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 bounds;

        private Vector3 resetPosition;
        private Vector2 dragPosition;
        
        private Transform target;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
            resetPosition = target ? target.position : Vector3.zero + offset;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            resetPosition = target ? target.position : Vector3.zero + offset;
        }

        public void LateUpdate()
        {
            Vector3 targetPosition = target ? target.position : Vector3.zero;

            if (Input.GetMouseButtonDown(1))
            {
                transform.position = resetPosition;
                dragPosition = Vector3.zero;
                return;
            }

            if (Input.GetMouseButton(2))
            {
                Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 previousMousePos = cam.ScreenToWorldPoint(Input.mousePosition - Input.mousePositionDelta);
                Vector2 difference = currentMousePos - previousMousePos;
                dragPosition -= difference * 2;
            }

            Vector2 screenBottomLeftWorldPos = cam.ScreenToWorldPoint(Vector2.zero) - transform.position;
            Vector2 screenTopRightWorldPos = cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, cam.pixelHeight)) - transform.position;

            dragPosition.x = Mathf.Clamp(dragPosition.x, -screenBottomLeftWorldPos.x - bounds.x, -screenTopRightWorldPos.x + bounds.x);
            dragPosition.y = Mathf.Clamp(dragPosition.y, -screenBottomLeftWorldPos.y - bounds.y, -screenTopRightWorldPos.y + bounds.y);

            transform.position = targetPosition + (Vector3)dragPosition + offset;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(target ? target.position : Vector2.zero, bounds * 2);
        }
    }
}
