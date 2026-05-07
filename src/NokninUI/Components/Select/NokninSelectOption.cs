namespace NokninUI.Components.Select;

public sealed class NokninSelectOption
{
    public string Value { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public bool Disabled { get; init; }
}