using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.TextArea;

public partial class NokninTextArea
{
    private readonly string _componentId = $"noknin-textarea-{Guid.NewGuid():N}";

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public string? Label { get; set; }

    [Parameter] public string? Placeholder { get; set; }

    [Parameter] public string? Description { get; set; }

    [Parameter] public string? ErrorText { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public bool ReadOnly { get; set; }

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public int Rows { get; set; } = 4;

    [Parameter] public int? MaxLength { get; set; }

    [Parameter] public bool ShowCharacterCount { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(ErrorText);
        }
    }

    private string Id
    {
        get
        {
            return $"{_componentId}-control";
        }
    }

    private string DescriptionId
    {
        get
        {
            return $"{_componentId}-description";
        }
    }

    private string ErrorId
    {
        get
        {
            return $"{_componentId}-error";
        }
    }

    private string? DescribedBy
    {
        get
        {
            if (HasError)
            {
                return ErrorId;
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                return DescriptionId;
            }

            return null;
        }
    }

    private int CurrentLength
    {
        get
        {
            return Value?.Length ?? 0;
        }
    }

    private string CharacterCountText
    {
        get
        {
            return MaxLength is null
            ? CurrentLength.ToString()
            : $"{CurrentLength}/{MaxLength}";
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
                "noknin-textarea",
                $"noknin-textarea--{Size.ToString().ToLowerInvariant()}",
                Disabled ? "noknin-textarea--disabled" : null,
                ReadOnly ? "noknin-textarea--readonly" : null,
                HasError ? "noknin-textarea--error" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    private async Task HandleInputAsync(ChangeEventArgs args)
    {
        if (Disabled || ReadOnly)
        {
            return;
        }

        Value = args.Value?.ToString();
        await ValueChanged.InvokeAsync(Value);
    }
}