using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Select;

public partial class NokninSelect
{
    private readonly string _id = $"noknin-select-{Guid.NewGuid():N}";
    private bool _open;

    [Parameter] public string? Label { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public IReadOnlyList<NokninSelectOption> Options { get; set; } = [];
    [Parameter] public string Placeholder { get; set; } = "Select an option";
    [Parameter] public string? Description { get; set; }
    [Parameter] public string? ErrorText { get; set; }
    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Required { get; set; }
    [Parameter] public string? Class { get; set; }

    private bool HasError
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
            return $"{_id}-label";
        }
    }

    private string DescriptionId
    {
        get
        {
            return $"{_id}-description";
        }
    }

    private string ErrorId
    {
        get
        {
            return $"{_id}-error";
        }
    }

    private string? DescribedBy
    {
        get
        {
            return HasError ? ErrorId :
        !string.IsNullOrWhiteSpace(Description) ? DescriptionId :
        null;
        }
    }

    private string LabelledBy
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Label) ? LabelId : string.Empty;
        }
    }

    private NokninSelectOption? SelectedOption
    {
        get
        {
            return Options.FirstOrDefault(x => x.Value == Value);
        }
    }

    private string SelectedLabel
    {
        get
        {
            return SelectedOption?.Label ?? Placeholder;
        }
    }

    private string ValueClassNames
    {
        get
        {
            return SelectedOption is null
            ? "noknin-select__value noknin-select__value--placeholder"
            : "noknin-select__value";
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-select noknin-select--{Size.ToString().ToLowerInvariant()} {(_open ? "noknin-select--open" : "")} {(Disabled ? "noknin-select--disabled" : "")} {(HasError ? "noknin-select--error" : "")} {Class}".Trim();
        }
    }

    private void Toggle()
    {
        if (Disabled)
        {
            return;
        }

        _open = !_open;
    }

    private async Task SelectAsync(NokninSelectOption option)
    {
        if (option.Disabled)
        {
            return;
        }

        Value = option.Value;
        _open = false;

        await ValueChanged.InvokeAsync(Value);
    }

    private void HandleTriggerKeyDown(KeyboardEventArgs args)
    {
        if (args.Key is "Enter" or " " or "ArrowDown")
        {
            _open = true;
        }

        if (args.Key == "Escape")
        {
            _open = false;
        }
    }

    private void HandleListKeyDown(KeyboardEventArgs args)
    {
        if (args.Key == "Escape")
        {
            _open = false;
        }
    }

    private string GetOptionClassNames(NokninSelectOption option)
    {
        var selected = option.Value == Value ? "noknin-select__option--selected" : "";
        var disabled = option.Disabled ? "noknin-select__option--disabled" : "";

        return $"noknin-select__option {selected} {disabled}".Trim();
    }
}