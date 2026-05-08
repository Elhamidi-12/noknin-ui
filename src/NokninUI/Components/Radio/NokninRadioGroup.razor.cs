using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Radio;

public partial class NokninRadioGroup
{
    private readonly string _componentId = $"noknin-radio-group-{Guid.NewGuid():N}";

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public string? Name { get; set; }

    [Parameter] public string? Label { get; set; }

    [Parameter] public string? Description { get; set; }

    [Parameter] public string? ErrorText { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public NokninRadioOrientation Orientation { get; set; } = NokninRadioOrientation.Vertical;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    internal string GroupName
    {
        get
        {
            return string.IsNullOrWhiteSpace(Name)
        ? _componentId
        : Name;
        }
    }

    internal bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(ErrorText);
        }
    }

    private string LabelId
    {
        get
        {
            return $"{_componentId}-label";
        }
    }

    private string? LabelledBy
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Label) ? LabelId : null;
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

    private string RootClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-radio-group",
                $"noknin-radio-group--{Size.ToString().ToLowerInvariant()}",
                $"noknin-radio-group--{Orientation.ToString().ToLowerInvariant()}",
                Disabled ? "noknin-radio-group--disabled" : null,
                HasError ? "noknin-radio-group--error" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    internal bool IsSelected(string? value)
    {
        return Value == value;
    }

    internal async Task SelectAsync(string? value)
    {
        if (Disabled)
        {
            return;
        }

        Value = value;
        await ValueChanged.InvokeAsync(Value);
    }
}
