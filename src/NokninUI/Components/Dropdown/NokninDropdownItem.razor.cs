using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace NokninUI.Components.Dropdown;

public partial class NokninDropdownItem
{
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter] public string? Class { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-dropdown-item {(Disabled ? "noknin-dropdown-item--disabled" : "")} {Class}".Trim();
        }
    }

    private async Task HandleClick(MouseEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        await OnClick.InvokeAsync(args);
    }
}