using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Table;

public partial class NokninTableCell
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public bool Header { get; set; }

    [Parameter] public string Scope { get; set; } = "col";

    [Parameter] public NokninTableCellAlign Align { get; set; } = NokninTableCellAlign.Start;

    [Parameter] public bool Numeric { get; set; }

    [Parameter] public bool NoWrap { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private NokninTableCellAlign EffectiveAlign
    {
        get
        {
            return Numeric ? NokninTableCellAlign.End : Align;
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
                Header ? "noknin-table__head-cell" : "noknin-table__cell",
                $"noknin-table__cell--align-{EffectiveAlign.ToString().ToLowerInvariant()}",
                Numeric ? "noknin-table__cell--numeric" : null,
                NoWrap ? "noknin-table__cell--nowrap" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}