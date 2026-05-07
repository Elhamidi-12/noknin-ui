using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Tabs;

public partial class NokninTabPanel
{
    [CascadingParameter] public NokninTabs? Tabs { get; set; }

    [Parameter, EditorRequired] public string Value { get; set; } = string.Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Lazy { get; set; }
    [Parameter] public string? Class { get; set; }

    private bool Selected
    {
        get
        {
            return Tabs?.CurrentValue == Value;
        }
    }

    private string TabId
    {
        get
        {
            return $"noknin-tab-{Value}";
        }
    }

    private string PanelId
    {
        get
        {
            return $"noknin-tab-panel-{Value}";
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-tab-panel {(Selected ? "noknin-tab-panel--selected" : "")} {Class}".Trim();
        }
    }
}