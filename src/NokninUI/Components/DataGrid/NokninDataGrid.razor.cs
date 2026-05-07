using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using NokninUI.Data.Enums;
using System.Reflection;

namespace NokninUI.Components.DataGrid;

public partial class NokninDataGrid<TItem>
{
    private readonly List<NokninDataGridColumn<TItem>> _columns = [];

    private string? _sortField;
    private NokninSortDirection _sortDirection = NokninSortDirection.None;

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

    [Parameter] public bool RowSelectable { get; set; }

    [Parameter] public TItem? SelectedItem { get; set; }

    [Parameter] public EventCallback<TItem?> SelectedItemChanged { get; set; }

    [Parameter] public bool ShowPagination { get; set; }

    [Parameter] public int Page { get; set; } = 1;

    [Parameter] public EventCallback<int> PageChanged { get; set; }

    [Parameter] public int PageSize { get; set; } = 10;

    [Parameter] public bool ShowPaginationSummary { get; set; } = true;

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    internal IReadOnlyList<NokninDataGridColumn<TItem>> RegisteredColumns
    {
        get
        {
            return _columns;
        }
    }

    private IReadOnlyList<TItem> SortedItems
    {
        get
        {
            return GetSortedItems();
        }
    }

    private IReadOnlyList<TItem> ResolvedItems
    {
        get
        {
            return GetResolvedItems();
        }
    }

    private int TotalItems
    {
        get
        {
            return SortedItems.Count;
        }
    }

    private int ColumnCount
    {
        get
        {
            return Math.Max(_columns.Count, 1);
        }
    }

    private bool ShouldShowPagination
    {
        get
        {
            return ShowPagination && !Loading && TotalItems > 0;
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
                RowSelectable ? "noknin-datagrid--selectable" : null,
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

    private IReadOnlyList<TItem> GetSortedItems()
    {
        var items = Items?.ToList() ?? [];

        if (string.IsNullOrWhiteSpace(_sortField) || _sortDirection == NokninSortDirection.None)
        {
            return items;
        }

        return _sortDirection == NokninSortDirection.Ascending
            ? items.OrderBy(item => GetComparableFieldValue(item, _sortField)).ToList()
            : items.OrderByDescending(item => GetComparableFieldValue(item, _sortField)).ToList();
    }

    private IReadOnlyList<TItem> GetResolvedItems()
    {
        var items = SortedItems;

        if (!ShowPagination || PageSize <= 0)
        {
            return items;
        }

        var totalPages = Math.Max(1, (int)Math.Ceiling((double)items.Count / PageSize));
        var currentPage = Math.Clamp(Page, 1, totalPages);

        return items
            .Skip((currentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    private async Task HandlePageChangedAsync(int page)
    {
        Page = page;
        await PageChanged.InvokeAsync(Page);
    }

    private async Task ToggleSortAsync(NokninDataGridColumn<TItem> column)
    {
        if (!column.CanSort || string.IsNullOrWhiteSpace(column.Field))
        {
            return;
        }

        if (_sortField != column.Field)
        {
            _sortField = column.Field;
            _sortDirection = NokninSortDirection.Ascending;
        }
        else
        {
            _sortDirection = _sortDirection switch
            {
                NokninSortDirection.None => NokninSortDirection.Ascending,
                NokninSortDirection.Ascending => NokninSortDirection.Descending,
                NokninSortDirection.Descending => NokninSortDirection.None,
                _ => NokninSortDirection.None
            };

            if (_sortDirection == NokninSortDirection.None)
            {
                _sortField = null;
            }
        }

        if (ShowPagination && Page != 1)
        {
            Page = 1;
            await PageChanged.InvokeAsync(Page);
        }
    }

    private async Task SelectRowAsync(TItem item)
    {
        if (!RowSelectable)
        {
            return;
        }

        SelectedItem = item;
        await SelectedItemChanged.InvokeAsync(SelectedItem);
    }

    private async Task HandleRowKeyDownAsync(KeyboardEventArgs args, TItem item)
    {
        if (!RowSelectable)
        {
            return;
        }

        if (args.Key is "Enter" or " ")
        {
            await SelectRowAsync(item);
        }
    }

    private bool IsSelected(TItem item)
    {
        return EqualityComparer<TItem>.Default.Equals(SelectedItem, item);
    }

    private string GetRowClass(TItem item)
    {
        return string.Join(
            " ",
            new[]
            {
                "noknin-datagrid__row",
                RowSelectable ? "noknin-datagrid__row--selectable" : null,
                IsSelected(item) ? "noknin-datagrid__row--selected" : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private string? GetRowTabIndex()
    {
        return RowSelectable ? "0" : null;
    }

    private string? GetRowRole()
    {
        return RowSelectable ? "button" : null;
    }

    private string? GetRowAriaSelected(TItem item)
    {
        return RowSelectable ? IsSelected(item).ToString().ToLowerInvariant() : null;
    }

    private string GetSortAriaSort(NokninDataGridColumn<TItem> column)
    {
        if (!column.CanSort || _sortField != column.Field)
        {
            return "none";
        }

        return _sortDirection switch
        {
            NokninSortDirection.Ascending => "ascending",
            NokninSortDirection.Descending => "descending",
            _ => "none"
        };
    }

    private string GetSortButtonLabel(NokninDataGridColumn<TItem> column)
    {
        if (!column.CanSort)
        {
            return column.Header;
        }

        if (_sortField != column.Field || _sortDirection == NokninSortDirection.None)
        {
            return $"Sort by {column.Header}";
        }

        return _sortDirection == NokninSortDirection.Ascending
            ? $"Sort by {column.Header}, currently ascending"
            : $"Sort by {column.Header}, currently descending";
    }

    private string GetSortIndicator(NokninDataGridColumn<TItem> column)
    {
        if (!column.CanSort || _sortField != column.Field)
        {
            return "↕";
        }

        return _sortDirection switch
        {
            NokninSortDirection.Ascending => "↑",
            NokninSortDirection.Descending => "↓",
            _ => "↕"
        };
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
                column.CanSort ? "noknin-datagrid__cell--sortable" : null,
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

    private static object? GetComparableFieldValue(TItem item, string fieldName)
    {
        var value = GetFieldValue(item, fieldName);

        if (value is null)
        {
            return null;
        }

        if (value is IComparable comparable)
        {
            return comparable;
        }

        return value.ToString();
    }
}