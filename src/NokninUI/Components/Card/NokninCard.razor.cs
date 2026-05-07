using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Card;

public partial class NokninCard
{
    [Parameter] public RenderFragment? Header { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    [Parameter] public NokninCardVariant Variant { get; set; } = NokninCardVariant.Elevated;
    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;
    [Parameter] public bool Interactive { get; set; }

    [Parameter] public string? Class { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-card noknin-card--{Variant.ToString().ToLowerInvariant()} noknin-card--{Size.ToString().ToLowerInvariant()} {(Interactive ? "noknin-card--interactive" : "")} {Class}".Trim();
        }
    }
}