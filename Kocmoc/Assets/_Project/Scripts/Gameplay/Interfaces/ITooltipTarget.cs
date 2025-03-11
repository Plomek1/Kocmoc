using System;
using System.Collections.Generic;

namespace Kocmoc.Gameplay
{
    public interface ITooltipTarget
    {
        Action TooltipUpdate {  get; }

        List<TooltipField> GetTooltipFields();
    }

    public struct TooltipField
    {
        public string header;
        public string text;
    }
}
