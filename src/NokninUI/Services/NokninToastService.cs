using NokninUI.Data.Enums;

namespace NokninUI.Services;

public sealed class NokninToastService
{
    public event Action<NokninToastMessage>? OnShow;
    public event Action<Guid>? OnDismiss;

    public void Show(string title, string? message = null, NokninToastVariant variant = NokninToastVariant.Info, int duration = 5000)
    {
        OnShow?.Invoke(new NokninToastMessage
        {
            Title = title,
            Message = message,
            Variant = variant,
            Duration = duration
        });
    }

    public void Success(string title, string? message = null, int duration = 5000)
    {
        Show(title, message, NokninToastVariant.Success, duration);
    }

    public void Error(string title, string? message = null, int duration = 5000)
    {
        Show(title, message, NokninToastVariant.Error, duration);
    }

    public void Warning(string title, string? message = null, int duration = 5000)
    {
        Show(title, message, NokninToastVariant.Warning, duration);
    }

    public void Info(string title, string? message = null, int duration = 5000)
    {
        Show(title, message, NokninToastVariant.Info, duration);
    }

    public void Dismiss(Guid id)
    {
        OnDismiss?.Invoke(id);
    }
}