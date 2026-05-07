using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;
using NokninUI.Services;

namespace NokninUI.Components.Toast;

public partial class NokninToastContainer : IDisposable
{
    private readonly List<NokninToastMessage> _toasts = [];

    [Parameter] public NokninToastPosition Position { get; set; } = NokninToastPosition.TopRight;
    [Parameter] public int MaxToasts { get; set; } = 5;
    [Parameter] public string? Class { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-toast-container noknin-toast-container--{Position.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }

    protected override void OnInitialized()
    {
        ToastService.OnShow += Show;
        ToastService.OnDismiss += Dismiss;
    }

    private void Show(NokninToastMessage message)
    {
        _toasts.Insert(0, message);

        if (_toasts.Count > MaxToasts)
        {
            _toasts.RemoveAt(_toasts.Count - 1);
        }

        _ = AutoDismissAsync(message);
        InvokeAsync(StateHasChanged);
    }

    private async Task AutoDismissAsync(NokninToastMessage message)
    {
        if (message.Duration <= 0)
        {
            return;
        }

        await Task.Delay(message.Duration);
        Dismiss(message.Id);
    }

    private Task DismissAsync(Guid id)
    {
        Dismiss(id);
        return Task.CompletedTask;
    }

    private void Dismiss(Guid id)
    {
        _toasts.RemoveAll(x => x.Id == id);
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ToastService.OnShow -= Show;
        ToastService.OnDismiss -= Dismiss;
    }
}