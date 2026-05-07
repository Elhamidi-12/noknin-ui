using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Checkbox;

public partial class NokninCheckbox
{
    private readonly string _componentId = $"noknin-checkbox-{Guid.NewGuid():N}";

    [Parameter] public bool Checked { get; set; }

    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

    [Parameter] public string? Label { get; set; }

    [Parameter] public string? Description { get; set; }

    [Parameter] public string? ErrorText { get; set; }

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool Required { get; set; }

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private bool HasError => !string.IsNullOrWhiteSpace(ErrorText);

    private string Id => $"{_componentId}-input";

    private string DescriptionId => $"{_componentId}-description";

    private string ErrorId => $"{_componentId}-error";

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

    private string RootClass =>
        string.Join(
            " ",
            new[]
            {
                "noknin-checkbox",
                $"noknin-checkbox--{Size.ToString().ToLowerInvariant()}",
                Checked ? "noknin-checkbox--checked" : null,
                Disabled ? "noknin-checkbox--disabled" : null,
                HasError ? "noknin-checkbox--error" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

    private async Task HandleChangeAsync(ChangeEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        var isChecked = args.Value is bool value && value;

        Checked = isChecked;
        await CheckedChanged.InvokeAsync(Checked);
    }
}