using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace NokninUI.Components.Table;

public partial class NokninTableRow
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool Selected { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private bool IsInteractive
    {
        get
        {
            return OnClick.HasDelegate && !Disabled;
        }
    }

    private string RootClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-table__row",
                Selected ? "noknin-table__row--selected" : null,
                Disabled ? "noknin-table__row--disabled" : null,
                IsInteractive ? "noknin-table__row--interactive" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    private async Task HandleClickAsync(MouseEventArgs args)
    {
        if (Disabled || !OnClick.HasDelegate)
        {
            return;
        }

        await OnClick.InvokeAsync(args);
    }
}