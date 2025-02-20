using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc.Gameplay
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Background[] backgrounds;

        private Vector2 lastFramePosition; 

        private void LateUpdate()
        {
            Vector2 positionDelta = (Vector2)transform.position - lastFramePosition;

            foreach (Background background in backgrounds)
            {
                background.renderer.transform.localPosition = (Vector2)background.renderer.transform.localPosition - positionDelta * background.parallaxFactor;
            }

            lastFramePosition = transform.position;
        }

        [System.Serializable]
        public struct Background
        {
            public SpriteRenderer renderer;
            public Vector2 parallaxFactor;
        }
    }
}
