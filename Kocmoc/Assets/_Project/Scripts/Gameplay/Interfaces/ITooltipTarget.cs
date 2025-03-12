using System.Collections.Generic;
using UnityEngine;
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
        public Color color;

        public string GetString() => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{header}: {text}</color>\n";

        public TooltipField (string header, string text)
        {
            this.header = header;
            this.text = text;
            this.color = Color.white;
        }

        public TooltipField (string header, string text, Color color)
        {
            this.header = header;
            this.text = text;
            this.color = color;
        }
    }
}
