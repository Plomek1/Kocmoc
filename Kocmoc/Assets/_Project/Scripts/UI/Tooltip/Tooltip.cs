using Kocmoc.Gameplay;
using UnityEngine;

namespace Kocmoc.UI
{
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private bool followMouse;
        [SerializeField] private Vector2 mouseOffset;
        [SerializeField] private float screenEdgePadding;

        protected ITooltipTarget target;

        public virtual void ShowTooltip(ITooltipTarget target)
        {
            this.target = target;
            ConnectTargetCallbacks(target);
            UpdateTooltip();
        }

        public virtual void HideTooltip()
        {
            DisconnectTargetCallbacks(target);
        }

        protected virtual void UpdateTooltip()
        {

        }

        protected void ConnectTargetCallbacks(ITooltipTarget target)
        {
            target.TooltipUpdate.AddListener(UpdateTooltip);
            target.TooltipDelete.AddListener(HideTooltip);
        }

        protected void DisconnectTargetCallbacks(ITooltipTarget target)
        {
            target.TooltipUpdate.RemoveListener(UpdateTooltip);
            target.TooltipDelete.RemoveListener(HideTooltip);
        }
    }
}
