using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Select;

public partial class NokninSelect
{
    private readonly string _id = $"noknin-select-{Guid.NewGuid():N}";
    private bool _open;
    private int _activeIndex = -1;

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
    [Parameter] public string? Style { get; set; }

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

    private string ListId
    {
        get
        {
            return $"{_id}-listbox";
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

    private string? LabelledBy
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Label) ? LabelId : null;
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

    private string? ActiveDescendant
    {
        get
        {
            return _open && _activeIndex >= 0 ? GetOptionId(_activeIndex) : null;
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

        if (_open)
        {
            Close();
            return;
        }

        Open();
    }

    private void Open()
    {
        if (Disabled)
        {
            return;
        }

        _open = true;
        _activeIndex = GetInitialActiveIndex();
    }

    private void Close()
    {
        _open = false;
        _activeIndex = -1;
    }

    private int GetInitialActiveIndex()
    {
        var selectedIndex = Options
            .Select((option, index) => new { option, index })
            .FirstOrDefault(x => x.option.Value == Value && !x.option.Disabled)
            ?.index;

        if (selectedIndex is not null)
        {
            return selectedIndex.Value;
        }

        return GetNextEnabledIndex(-1, 1);
    }

    private async Task SelectAsync(NokninSelectOption option)
    {
        if (option.Disabled)
        {
            return;
        }

        Value = option.Value;
        Close();

        await ValueChanged.InvokeAsync(Value);
    }

    private async Task SelectActiveAsync()
    {
        if (_activeIndex < 0 || _activeIndex >= Options.Count)
        {
            return;
        }

        var option = Options[_activeIndex];

        if (option.Disabled)
        {
            return;
        }

        await SelectAsync(option);
    }

    private async Task HandleTriggerKeyDown(KeyboardEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        switch (args.Key)
        {
            case "Enter":
            case " ":
                if (!_open)
                {
                    Open();
                }
                else
                {
                    await SelectActiveAsync();
                }
                break;

            case "ArrowDown":
                if (!_open)
                {
                    Open();
                }
                else
                {
                    MoveActive(1);
                }
                break;

            case "ArrowUp":
                if (!_open)
                {
                    Open();
                }
                else
                {
                    MoveActive(-1);
                }
                break;

            case "Home":
                if (!_open)
                {
                    Open();
                }

                MoveToFirst();
                break;

            case "End":
                if (!_open)
                {
                    Open();
                }

                MoveToLast();
                break;

            case "Escape":
                Close();
                break;
        }
    }

    private async Task HandleListKeyDown(KeyboardEventArgs args)
    {
        switch (args.Key)
        {
            case "Enter":
            case " ":
                await SelectActiveAsync();
                break;

            case "ArrowDown":
                MoveActive(1);
                break;

            case "ArrowUp":
                MoveActive(-1);
                break;

            case "Home":
                MoveToFirst();
                break;

            case "End":
                MoveToLast();
                break;

            case "Escape":
                Close();
                break;
        }
    }

    private void MoveActive(int direction)
    {
        if (Options.Count == 0)
        {
            _activeIndex = -1;
            return;
        }

        _activeIndex = GetNextEnabledIndex(_activeIndex, direction);
    }

    private void MoveToFirst()
    {
        _activeIndex = GetNextEnabledIndex(-1, 1);
    }

    private void MoveToLast()
    {
        _activeIndex = GetNextEnabledIndex(Options.Count, -1);
    }

    private int GetNextEnabledIndex(int startIndex, int direction)
    {
        if (Options.Count == 0)
        {
            return -1;
        }

        var index = startIndex;

        for (var i = 0; i < Options.Count; i++)
        {
            index += direction;

            if (index < 0)
            {
                index = Options.Count - 1;
            }

            if (index >= Options.Count)
            {
                index = 0;
            }

            if (!Options[index].Disabled)
            {
                return index;
            }
        }

        return -1;
    }

    private string GetOptionId(int index)
    {
        return $"{_id}-option-{index}";
    }

    private string GetOptionClassNames(NokninSelectOption option, int index)
    {
        var selected = option.Value == Value ? "noknin-select__option--selected" : "";
        var active = index == _activeIndex ? "noknin-select__option--active" : "";
        var disabled = option.Disabled ? "noknin-select__option--disabled" : "";

        return $"noknin-select__option {selected} {active} {disabled}".Trim();
    }
}
