using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Button;

public partial class NokninButton
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public NokninVariant Variant { get; set; } = NokninVariant.Primary;
    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? AriaLabel { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    private string ClassNames =>
        $"noknin-button noknin-button--{Variant.ToString().ToLowerInvariant()} noknin-button--{Size.ToString().ToLowerInvariant()} {Class}".Trim();
}