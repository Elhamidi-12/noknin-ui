using NokninUI.Data.Enums;

namespace NokninUI.Services;

public sealed class NokninToastMessage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string? Message { get; init; }
    public NokninToastVariant Variant { get; init; } = NokninToastVariant.Info;
    public int Duration { get; init; } = 5000;
}