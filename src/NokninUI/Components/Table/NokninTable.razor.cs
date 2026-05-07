using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Table;

public partial class NokninTable
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Caption { get; set; }

    [Parameter] public NokninTableVariant Variant { get; set; } = NokninTableVariant.Default;

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public bool Hoverable { get; set; }

    [Parameter] public bool Compact { get; set; }

    [Parameter] public bool Responsive { get; set; } = true;

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private string WrapperClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-table-wrapper",
                Responsive ? "noknin-table-wrapper--responsive" : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
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
                "noknin-table",
                $"noknin-table--{Variant.ToString().ToLowerInvariant()}",
                $"noknin-table--{Size.ToString().ToLowerInvariant()}",
                Compact ? "noknin-table--compact" : null,
                Hoverable ? "noknin-table--hoverable" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}