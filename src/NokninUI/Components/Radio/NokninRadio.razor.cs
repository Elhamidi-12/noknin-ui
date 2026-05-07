using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Radio;

public partial class NokninRadio
{
    private readonly string _componentId = $"noknin-radio-{Guid.NewGuid():N}";

    [CascadingParameter] internal NokninRadioGroup? RadioGroup { get; set; }

    [Parameter] public string? Value { get; set; }

    [Parameter] public string? Label { get; set; }

    [Parameter] public string? Description { get; set; }

    [Parameter] public string? ErrorText { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public NokninSize? Size { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private bool IsChecked
    {
        get
        {
            return RadioGroup?.IsSelected(Value) == true;
        }
    }

    private bool IsDisabled
    {
        get
        {
            return Disabled || RadioGroup?.Disabled == true;
        }
    }

    private bool IsRequired
    {
        get
        {
            return Required || RadioGroup?.Required == true;
        }
    }

    private bool ShowRequiredIndicator
    {
        get
        {
            return Required && RadioGroup?.Required != true;
        }
    }

    private bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(ErrorText) || RadioGroup?.HasError == true;
        }
    }

    private NokninSize EffectiveSize
    {
        get
        {
            return Size ?? RadioGroup?.Size ?? NokninSize.Medium;
        }
    }

    private string GroupName
    {
        get
        {
            return RadioGroup?.GroupName ?? _componentId;
        }
    }

    private string Id
    {
        get
        {
            return $"{_componentId}-input";
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
            if (!string.IsNullOrWhiteSpace(ErrorText))
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
                "noknin-radio",
                $"noknin-radio--{EffectiveSize.ToString().ToLowerInvariant()}",
                IsChecked ? "noknin-radio--checked" : null,
                IsDisabled ? "noknin-radio--disabled" : null,
                HasError ? "noknin-radio--error" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    private async Task HandleChangeAsync(ChangeEventArgs args)
    {
        if (IsDisabled || RadioGroup is null)
        {
            return;
        }

        await RadioGroup.SelectAsync(Value);
    }
}