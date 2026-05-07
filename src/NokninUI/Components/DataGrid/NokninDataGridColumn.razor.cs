using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.DataGrid;

public partial class NokninDataGridColumn<TItem> : ComponentBase, IDisposable
{
    [CascadingParameter] internal NokninDataGrid<TItem>? DataGrid { get; set; }

    [Parameter] public string Header { get; set; } = string.Empty;

    [Parameter] public string? Field { get; set; }

    [Parameter] public RenderFragment<TItem>? Template { get; set; }

    [Parameter] public NokninTableCellAlign Align { get; set; } = NokninTableCellAlign.Start;

    [Parameter] public bool Numeric { get; set; }

    [Parameter] public bool NoWrap { get; set; }

    [Parameter] public string? Class { get; set; }

    internal NokninTableCellAlign EffectiveAlign
    {
        get
        {
            return Numeric ? NokninTableCellAlign.End : Align;
        }
    }

    protected override void OnInitialized()
    {
        DataGrid?.RegisterColumn(this);
    }

    public void Dispose()
    {
        DataGrid?.UnregisterColumn(this);
    }
}