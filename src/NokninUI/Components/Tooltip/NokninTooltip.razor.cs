using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Tooltip;

public partial class NokninTooltip
{
    private readonly string _tooltipId = $"noknin-tooltip-{Guid.NewGuid():N}";
    private bool _open;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Text { get; set; }
    [Parameter] public NokninTooltipPosition Position { get; set; } = NokninTooltipPosition.Top;
    [Parameter] public string? Class { get; set; }

    private string TooltipId
    {
        get
        {
            return _tooltipId;
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-tooltip noknin-tooltip--{Position.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }

    private void Show()
    {
        _open = true;
    }

    private void Hide()
    {
        _open = false;
    }
}