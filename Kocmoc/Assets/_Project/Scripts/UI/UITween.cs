using UnityEngine;
using DG.Tweening;

namespace Kocmoc
{
    public class UITween : MonoBehaviour
    {
        [SerializeField] private float enterDuration;
        [SerializeField] private Vector2 enterPos;
        private Vector2 startPos;

        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            startPos = rectTransform.anchoredPosition;
        }

        public void PlayEnterTween()
        {
            rectTransform.DOAnchorPos(enterPos, enterDuration).SetEase(Ease.OutCubic);
        }

        public void PlayExitTween()
        {
            rectTransform.DOAnchorPos(startPos, enterDuration).SetEase(Ease.InCubic);
        }

    }
}
