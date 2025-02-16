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
            

            if (Input.GetMouseButton(2))
            {
                Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 previousMousePos = cam.ScreenToWorldPoint(Input.mousePosition - Input.mousePositionDelta);
                Vector2 difference = currentMousePos - previousMousePos;

                dragPosition -= difference * 2;
                dragPosition.x = Mathf.Clamp(dragPosition.x, -bounds.x, bounds.x);
                dragPosition.y = Mathf.Clamp(dragPosition.y, -bounds.y, bounds.y);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                transform.position = resetPosition;
                dragPosition = Vector3.zero;
                return;
            }

            transform.position = targetPosition + (Vector3)dragPosition + offset;
        }
    }
}
