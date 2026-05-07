using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;
using System.Reflection;

namespace NokninUI.Components.DataGrid;

public partial class NokninDataGrid<TItem>
{
    private readonly List<NokninDataGridColumn<TItem>> _columns = [];

    [Parameter] public IEnumerable<TItem>? Items { get; set; }

    [Parameter] public RenderFragment? Columns { get; set; }

    [Parameter] public RenderFragment? EmptyContent { get; set; }

    [Parameter] public RenderFragment? LoadingContent { get; set; }

    [Parameter] public string? Caption { get; set; }

    [Parameter] public string EmptyText { get; set; } = "No data available.";

    [Parameter] public string LoadingText { get; set; } = "Loading...";

    [Parameter] public bool Loading { get; set; }

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public bool Striped { get; set; }

    [Parameter] public bool Bordered { get; set; }

    [Parameter] public bool Hoverable { get; set; }

    [Parameter] public bool Responsive { get; set; } = true;

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    internal IReadOnlyList<NokninDataGridColumn<TItem>> RegisteredColumns
    {
        get
        {
            return _columns;
        }
    }

    private IReadOnlyList<TItem> ResolvedItems
    {
        get
        {
            return Items?.ToList() ?? [];
        }
    }

    private int ColumnCount
    {
        get
        {
            return Math.Max(_columns.Count, 1);
        }
    }

    private string WrapperClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-datagrid-wrapper",
                Responsive ? "noknin-datagrid-wrapper--responsive" : null
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
                "noknin-datagrid",
                $"noknin-datagrid--{Size.ToString().ToLowerInvariant()}",
                Striped ? "noknin-datagrid--striped" : null,
                Bordered ? "noknin-datagrid--bordered" : null,
                Hoverable ? "noknin-datagrid--hoverable" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    internal void RegisterColumn(NokninDataGridColumn<TItem> column)
    {
        if (_columns.Any(existingColumn => ReferenceEquals(existingColumn, column)))
        {
            return;
        }

        _columns.Add(column);
        StateHasChanged();
    }

    internal void UnregisterColumn(NokninDataGridColumn<TItem> column)
    {
        if (_columns.Remove(column))
        {
            StateHasChanged();
        }
    }

    private static string GetHeaderCellClass(NokninDataGridColumn<TItem> column)
    {
        return GetCellClass("noknin-datagrid__head-cell", column);
    }

    private static string GetBodyCellClass(NokninDataGridColumn<TItem> column)
    {
        return GetCellClass("noknin-datagrid__cell", column);
    }

    private static string GetCellClass(string baseClass, NokninDataGridColumn<TItem> column)
    {
        return string.Join(
            " ",
            new[]
            {
                baseClass,
                $"noknin-datagrid__cell--align-{column.EffectiveAlign.ToString().ToLowerInvariant()}",
                column.Numeric ? "noknin-datagrid__cell--numeric" : null,
                column.NoWrap ? "noknin-datagrid__cell--nowrap" : null,
                column.Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private static object? GetFieldValue(TItem item, string fieldName)
    {
        if (item is null || string.IsNullOrWhiteSpace(fieldName))
        {
            return null;
        }

        var property = typeof(TItem).GetProperty(
            fieldName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

        return property?.GetValue(item);
    }
}