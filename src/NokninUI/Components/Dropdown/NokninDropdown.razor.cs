using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Dropdown;

public partial class NokninDropdown
{
    [Parameter] public RenderFragment? Trigger { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public NokninDropdownAlign Align { get; set; } = NokninDropdownAlign.Start;
    [Parameter] public bool Open { get; set; }
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }

    [Parameter] public string? Class { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-dropdown noknin-dropdown--{Align.ToString().ToLowerInvariant()} {(Open ? "noknin-dropdown--open" : "")} {Class}".Trim();
        }
    }

    private async Task ToggleAsync()
    {
        Open = !Open;
        await OpenChanged.InvokeAsync(Open);
    }

    private async Task CloseAsync()
    {
        Open = false;
        await OpenChanged.InvokeAsync(false);
    }

    private async Task HandleTriggerKeyDown(KeyboardEventArgs args)
    {
        if (args.Key is "ArrowDown" or "Enter" or " ")
        {
            Open = true;
            await OpenChanged.InvokeAsync(true);
        }

        if (args.Key == "Escape")
        {
            await CloseAsync();
        }
    }

    private async Task HandleMenuKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Escape")
        {
            await CloseAsync();
        }
    }
}