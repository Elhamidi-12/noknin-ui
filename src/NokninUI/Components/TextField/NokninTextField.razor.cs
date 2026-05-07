using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.TextField;

public partial class NokninTextField
{
    private readonly string _generatedId = $"noknin-textfield-{Guid.NewGuid():N}";

    [Parameter] public string? Id { get; set; }
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public string? ErrorText { get; set; }
    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Required { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }

    private bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(ErrorText);
        }
    }

    private string InputId
    {
        get
        {
            return Id ?? _generatedId;
        }
    }

    private string DescriptionId
    {
        get
        {
            return $"{InputId}-description";
        }
    }

    private string ErrorId
    {
        get
        {
            return $"{InputId}-error";
        }
    }

    private string? DescribedBy
    {
        get
        {
            return HasError ? ErrorId : !string.IsNullOrWhiteSpace(Description) ? DescriptionId : null;
        }
    }

    private string WrapperClassNames
    {
        get
        {
            return $"noknin-textfield noknin-textfield--{Size.ToString().ToLowerInvariant()} {(Disabled ? "noknin-textfield--disabled" : "")} {(HasError ? "noknin-textfield--error" : "")} {Class}".Trim();
        }
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        Value = args.Value?.ToString();
        await ValueChanged.InvokeAsync(Value);
    }
}
