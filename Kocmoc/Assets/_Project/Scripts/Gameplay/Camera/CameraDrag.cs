using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class CameraDrag : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 bounds;

        private bool dragging;

        private Vector3 dragPosition;
        private Vector3 resetPosition;
        private Vector3 origin;
        private Vector3 difference;
        
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
            
            if(Input.GetMouseButtonDown(1))
            {
                transform.position = resetPosition;
                dragPosition = Vector3.zero;
                return;
            }

            if (Input.GetMouseButton(2))
            {
                difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                
                if(!dragging)
                {
                    origin = cam.ScreenToWorldPoint(Input.mousePosition);
                    dragging = true;
                }
            }
            else dragging = false;


            if (dragging)
            {
                dragPosition = origin - difference - targetPosition;
                dragPosition.x = Mathf.Clamp(dragPosition.x, -bounds.x, bounds.x);
                dragPosition.y = Mathf.Clamp(dragPosition.y, -bounds.y, bounds.y);
            }

            transform.position = targetPosition + dragPosition + offset;
        }
    }
}
