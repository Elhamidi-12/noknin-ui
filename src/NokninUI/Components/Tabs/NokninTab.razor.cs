using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Tabs;

public partial class NokninTab
{
    [CascadingParameter] public NokninTabs? Tabs { get; set; }

    [Parameter, EditorRequired] public string Value { get; set; } = string.Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public bool Disabled { get; set; }
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

    private int TabIndex
    {
        get
        {
            return Selected ? 0 : -1;
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-tab {(Selected ? "noknin-tab--selected" : "")} {(Disabled ? "noknin-tab--disabled" : "")} {Class}".Trim();
        }
    }

    private async Task SelectAsync()
    {
        if (Disabled || Tabs is null)
        {
            return;
        }

        await Tabs.SelectAsync(Value);
    }
}