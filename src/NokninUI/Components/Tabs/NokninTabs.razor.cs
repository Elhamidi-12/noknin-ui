using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Tabs;

public partial class NokninTabs
{
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public RenderFragment? Tabs { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public NokninTabsOrientation Orientation { get; set; } = NokninTabsOrientation.Horizontal;

    [Parameter] public string? Class { get; set; }

    internal string? CurrentValue => Value;

    internal async Task SelectAsync(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }

    private string AriaOrientation
    {
        get
        {
            return Orientation == NokninTabsOrientation.Vertical ? "vertical" : "horizontal";
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-tabs noknin-tabs--{Orientation.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }
}