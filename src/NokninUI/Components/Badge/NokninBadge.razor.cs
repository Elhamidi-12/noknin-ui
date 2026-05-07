using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Badge;

public partial class NokninBadge
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public NokninBadgeVariant Variant { get; set; } = NokninBadgeVariant.Neutral;
    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;
    [Parameter] public string? Class { get; set; }

    /// <summary>
    /// Optional ARIA role. Use "status" for dynamic status updates.
    /// </summary>
    [Parameter] public string? Role { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-badge noknin-badge--{Variant.ToString().ToLowerInvariant()} noknin-badge--{Size.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }
}