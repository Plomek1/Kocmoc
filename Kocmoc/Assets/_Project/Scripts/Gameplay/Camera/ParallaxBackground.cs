using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 parallaxFactor;


        private Transform cameraTransform;
        private Vector2 lastCameraPosition;

        private float textureUnitSizeX;
        private float textureUnitSizeY;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            CalculateUnitSize();
        }

        private void LateUpdate()
        {
            Vector2 positionDelta = (Vector2)cameraTransform.position - lastCameraPosition;
            transform.position += (Vector3)(positionDelta * parallaxFactor);
            lastCameraPosition = cameraTransform.position;

            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
            }

            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
            }
        }

        public void CalculateUnitSize()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            textureUnitSizeX = renderer.sprite.texture.width / renderer.sprite.pixelsPerUnit;
            textureUnitSizeY = renderer.sprite.texture.height / renderer.sprite.pixelsPerUnit;
        }
    }
}
