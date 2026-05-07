using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Modal;

public partial class NokninModal
{
    private readonly string _titleId = $"noknin-modal-title-{Guid.NewGuid():N}";

    [Parameter] public bool Open { get; set; }
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }

    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }

    [Parameter] public NokninModalSize Size { get; set; } = NokninModalSize.Medium;

    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;
    [Parameter] public bool CloseOnEscape { get; set; } = true;
    [Parameter] public bool ShowCloseButton { get; set; } = true;

    [Parameter] public EventCallback OnClose { get; set; }

    [Parameter] public string? Class { get; set; }

    private string TitleId
    {
        get
        {
            return _titleId;
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-modal__dialog noknin-modal__dialog--{Size.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }

    private async Task CloseAsync()
    {
        Open = false;

        await OpenChanged.InvokeAsync(false);
        await OnClose.InvokeAsync();
    }

    private async Task HandleBackdropClick()
    {
        if (CloseOnBackdropClick)
        {
            await CloseAsync();
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs args)
    {
        if (CloseOnEscape && args.Key == "Escape")
        {
            await CloseAsync();
        }
    }
}