using UnityEngine;
using DG.Tweening;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        public bool followTarget;
        public Transform target;
        [Space(10)]

        public bool boundCamera;
        public Vector3 bounds;
        [Space(10)]
        
        [SerializeField] private Vector3 offset;

        private Vector3 lastTargetPosition;
        private Vector3 resetPosition => (Vector3)GetTargetPosition() + offset;
        private Vector2 dragPosition;
        
        private Camera cam;
        private bool dragging;

        public void ResetPosition()
        {
            transform.DOMove(resetPosition, .3f).SetEase(Ease.OutCubic);
            dragPosition = Vector3.zero;
        }

        private void Start()
        {
            cam = GetComponent<Camera>();
            transform.position = resetPosition;
            Assets.Instance.inputReader.CameraDrag += performed => dragging = performed;
            Assets.Instance.inputReader.CameraReset += ResetPosition;
        }

        private void LateUpdate()
        {
            Vector2 lastDragPosition = dragPosition;

            if (dragging)
            {
                Vector2 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector2 previousMousePos = cam.ScreenToWorldPoint(Input.mousePosition - Input.mousePositionDelta);
                Vector2 difference = currentMousePos - previousMousePos;
                dragPosition -= difference * 2;
            }

            if(boundCamera)
            {
                Vector2 screenBottomLeftWorldPos = cam.ScreenToWorldPoint(Vector2.zero) - transform.position;
                Vector2 screenTopRightWorldPos = cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, cam.pixelHeight)) - transform.position;

                dragPosition.x = Mathf.Clamp(dragPosition.x, -screenBottomLeftWorldPos.x - bounds.x, -screenTopRightWorldPos.x + bounds.x);
                dragPosition.y = Mathf.Clamp(dragPosition.y, -screenBottomLeftWorldPos.y - bounds.y, -screenTopRightWorldPos.y + bounds.y);
            }

            Vector3 dragPositionDelta = dragPosition - lastDragPosition;
            Vector3 targetPosition = GetTargetPosition();

            if (followTarget)
            {
                Vector3 targetPositionDelta = targetPosition - lastTargetPosition;
                transform.position += targetPositionDelta;
            }
            
            transform.position += dragPositionDelta;
            lastTargetPosition = targetPosition;
        }

        private Vector2 GetTargetPosition() => target ? target.position : Vector3.zero;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(target ? target.position : Vector2.zero, bounds * 2);
        }
    }
}
