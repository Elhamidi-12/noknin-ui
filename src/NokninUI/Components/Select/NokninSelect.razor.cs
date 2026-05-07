using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Select;

public partial class NokninSelect : IAsyncDisposable
{
    private readonly string _componentId = $"noknin-select-{Guid.NewGuid():N}";

    private ElementReference _rootElement;
    private DotNetObjectReference<NokninSelect>? _dotNetReference;
    private IJSObjectReference? _module;

    private bool IsOpen { get; set; }
    private int _activeIndex = -1;

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public string? Label { get; set; }

    [Parameter] public IReadOnlyList<NokninSelectOption> Options { get; set; } = Array.Empty<NokninSelectOption>();

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    [Parameter] public string? Placeholder { get; set; } = "Select an option";

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Required { get; set; }
    [Parameter] public string? Description { get; set; }

    [Parameter] public string? ErrorText { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(ErrorText);
        }
    }

    private string TriggerId
    {
        get
        {
            return $"{_componentId}-trigger";
        }
    }

    private string LabelId
    {
        get
        {
            return $"{_componentId}-label";
        }
    }

    private string ListboxId
    {
        get
        {
            return $"{_componentId}-listbox";
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

    private string? ActiveDescendantId
    {
        get
        {
            return IsOpen && IsValidActiveIndex(_activeIndex)
            ? GetOptionId(_activeIndex)
            : null;
        }
    }

    private string AriaLabelledBy
    {
        get
        {
            return string.IsNullOrWhiteSpace(Label)
            ? TriggerId
            : $"{LabelId} {TriggerId}";
        }
    }

    private NokninSelectOption? SelectedOption
    {
        get
        {
            return Options.FirstOrDefault(option => option.Value == Value);
        }
    }

    private string SelectedLabel
    {
        get
        {
            return SelectedOption?.Label
        ?? Placeholder
        ?? string.Empty;
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
                "noknin-select",
                $"noknin-select--{Size.ToString().ToLowerInvariant()}",
                IsOpen ? "noknin-select--open" : null,
                Disabled ? "noknin-select--disabled" : null,
                HasError ? "noknin-select--error" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    private string ValueClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-select__value",
                SelectedOption is null ? "noknin-select__value--placeholder" : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _dotNetReference = DotNetObjectReference.Create(this);

        _module = await JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/NokninUI/js/noknin-select.js");

        await _module.InvokeVoidAsync(
            "registerOutsideClick",
            _rootElement,
            _dotNetReference);
    }

    private async Task ToggleAsync()
    {
        if (Disabled)
        {
            return;
        }

        if (IsOpen)
        {
            Close();
        }
        else
        {
            Open();
        }

        await Task.CompletedTask;
    }

    private void Open()
    {
        if (Disabled || IsOpen)
        {
            return;
        }

        IsOpen = true;
        SetInitialActiveIndex();
    }

    private void Close()
    {
        IsOpen = false;
        _activeIndex = -1;
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        switch (args.Key)
        {
            case "ArrowDown":
                if (!IsOpen)
                {
                    Open();
                }
                else
                {
                    MoveActiveIndex(1);
                }

                break;

            case "ArrowUp":
                if (!IsOpen)
                {
                    Open();
                }
                else
                {
                    MoveActiveIndex(-1);
                }

                break;

            case "Home":
                if (!IsOpen)
                {
                    Open();
                }

                SetFirstEnabledActiveIndex();
                break;

            case "End":
                if (!IsOpen)
                {
                    Open();
                }

                SetLastEnabledActiveIndex();
                break;

            case "Enter":
            case " ":
                if (!IsOpen)
                {
                    Open();
                }
                else if (IsValidActiveIndex(_activeIndex))
                {
                    await SelectOptionAsync(_activeIndex);
                }

                break;

            case "Escape":
                Close();
                break;

            case "Tab":
                Close();
                break;
        }
    }

    private async Task SelectOptionAsync(int index)
    {
        if (!IsValidActiveIndex(index))
        {
            return;
        }

        var option = Options[index];

        if (option.Disabled)
        {
            return;
        }

        Value = option.Value;
        await ValueChanged.InvokeAsync(Value);

        Close();
    }

    private void SetInitialActiveIndex()
    {
        var selectedIndex = Options.ToList().FindIndex(option => option.Value == Value && !option.Disabled);

        if (selectedIndex >= 0)
        {
            _activeIndex = selectedIndex;
            return;
        }

        SetFirstEnabledActiveIndex();
    }

    private void SetFirstEnabledActiveIndex()
    {
        _activeIndex = Options.ToList().FindIndex(option => !option.Disabled);
    }

    private void SetLastEnabledActiveIndex()
    {
        for (var index = Options.Count - 1; index >= 0; index--)
        {
            if (!Options[index].Disabled)
            {
                _activeIndex = index;
                return;
            }
        }

        _activeIndex = -1;
    }

    private void MoveActiveIndex(int direction)
    {
        if (Options.Count == 0)
        {
            _activeIndex = -1;
            return;
        }

        var startIndex = IsValidActiveIndex(_activeIndex)
            ? _activeIndex
            : direction > 0
                ? -1
                : Options.Count;

        for (var step = 1; step <= Options.Count; step++)
        {
            var nextIndex = (startIndex + direction * step + Options.Count) % Options.Count;

            if (!Options[nextIndex].Disabled)
            {
                _activeIndex = nextIndex;
                return;
            }
        }

        _activeIndex = -1;
    }

    private bool IsValidActiveIndex(int index)
    {
        return index >= 0 && index < Options.Count;
    }

    private bool IsSelected(NokninSelectOption option)
    {
        return option.Value == Value;
    }

    private string GetOptionId(int index)
    {
        return $"{_componentId}-option-{index}";
    }

    private string GetOptionClass(NokninSelectOption option, int index)
    {
        return string.Join(
            " ",
            new[]
            {
                "noknin-select__option",
                IsSelected(option) ? "noknin-select__option--selected" : null,
                index == _activeIndex ? "noknin-select__option--active" : null,
                option.Disabled ? "noknin-select__option--disabled" : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    [JSInvokable]
    public Task CloseFromOutsideClickAsync()
    {
        if (!IsOpen)
        {
            return Task.CompletedTask;
        }

        Close();
        StateHasChanged();

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync("unregisterOutsideClick", _rootElement);
                await _module.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
            }
        }

        _dotNetReference?.Dispose();
    }
}
