using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Kocmoc.Gameplay
{
    public interface ITooltipTarget
    {
        UnityEvent TooltipUpdate { get; }
        UnityEvent TooltipDelete { get; }

        List<TooltipField> GetTooltipFields();
    }

    public struct TooltipField
    {
        public string header;
        public string text;
    }
}
