using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Tabs;

public partial class NokninTabPanel
{
    [CascadingParameter] public NokninTabs? Tabs { get; set; }

    [Parameter, EditorRequired] public string Value { get; set; } = string.Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Lazy { get; set; }
    [Parameter] public string? Class { get; set; }

    private bool Selected => Tabs?.CurrentValue == Value;

    private string TabId => $"noknin-tab-{Value}";
    private string PanelId => $"noknin-tab-panel-{Value}";

    private string ClassNames
    {
        get
        {
            return $"noknin-tab-panel {(Selected ? "noknin-tab-panel--selected" : "")} {Class}".Trim();
        }
    }
}