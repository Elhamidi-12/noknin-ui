using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Pagination;

public partial class NokninPagination
{
    [Parameter] public int Page { get; set; } = 1;

    [Parameter] public EventCallback<int> PageChanged { get; set; }

    [Parameter] public int TotalItems { get; set; }

    [Parameter] public int PageSize { get; set; } = 10;

    [Parameter] public int SiblingCount { get; set; } = 1;

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public bool Disabled { get; set; }

    [Parameter] public bool ShowSummary { get; set; } = true;

    [Parameter] public string AriaLabel { get; set; } = "Pagination";

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private int TotalPages
    {
        get
        {
            if (PageSize <= 0 || TotalItems <= 0)
            {
                return 1;
            }

            return (int)Math.Ceiling((double)TotalItems / PageSize);
        }
    }

    private int CurrentPage
    {
        get
        {
            return Math.Clamp(Page, 1, TotalPages);
        }
    }

    private bool IsPreviousDisabled
    {
        get
        {
            return Disabled || CurrentPage <= 1;
        }
    }

    private bool IsNextDisabled
    {
        get
        {
            return Disabled || CurrentPage >= TotalPages;
        }
    }

    private int StartItem
    {
        get
        {
            if (TotalItems <= 0)
            {
                return 0;
            }

            return ((CurrentPage - 1) * PageSize) + 1;
        }
    }

    private int EndItem
    {
        get
        {
            if (TotalItems <= 0)
            {
                return 0;
            }

            return Math.Min(CurrentPage * PageSize, TotalItems);
        }
    }

    private string SummaryText
    {
        get
        {
            return ShowSummary
            ? $"{StartItem}-{EndItem} of {TotalItems}"
            : string.Empty;
        }
    }

    private IReadOnlyList<int?> VisiblePages
    {
        get
        {
            return GetVisiblePages();
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
                "noknin-pagination",
                $"noknin-pagination--{Size.ToString().ToLowerInvariant()}",
                Disabled ? "noknin-pagination--disabled" : null,
                !ShowSummary ? "noknin-pagination--no-summary" : null,
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }

    private async Task GoToPreviousPageAsync()
    {
        await GoToPageAsync(CurrentPage - 1);
    }

    private async Task GoToNextPageAsync()
    {
        await GoToPageAsync(CurrentPage + 1);
    }

    private async Task GoToPageAsync(int page)
    {
        if (Disabled)
        {
            return;
        }

        var nextPage = Math.Clamp(page, 1, TotalPages);

        if (nextPage == CurrentPage)
        {
            return;
        }

        Page = nextPage;
        await PageChanged.InvokeAsync(Page);
    }

    private string GetPageButtonClass(int pageNumber)
    {
        return string.Join(
            " ",
            new[]
            {
                "noknin-pagination__page",
                pageNumber == CurrentPage ? "noknin-pagination__page--active" : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private string GetPageAriaLabel(int pageNumber)
    {
        return pageNumber == CurrentPage
            ? $"Page {pageNumber}, current page"
            : $"Go to page {pageNumber}";
    }

    private IReadOnlyList<int?> GetVisiblePages()
    {
        var totalPages = TotalPages;
        var currentPage = CurrentPage;
        var siblingCount = Math.Max(0, SiblingCount);

        if (totalPages <= 7 + siblingCount * 2)
        {
            return Enumerable.Range(1, totalPages).Select<int, int?>(page => page).ToList();
        }

        var pages = new List<int?>();

        var leftSibling = Math.Max(currentPage - siblingCount, 1);
        var rightSibling = Math.Min(currentPage + siblingCount, totalPages);

        var showLeftEllipsis = leftSibling > 2;
        var showRightEllipsis = rightSibling < totalPages - 1;

        pages.Add(1);

        if (showLeftEllipsis)
        {
            pages.Add(null);
        }
        else
        {
            for (var page = 2; page < leftSibling; page++)
            {
                pages.Add(page);
            }
        }

        for (var page = leftSibling; page <= rightSibling; page++)
        {
            if (page != 1 && page != totalPages)
            {
                pages.Add(page);
            }
        }

        if (showRightEllipsis)
        {
            pages.Add(null);
        }
        else
        {
            for (var page = rightSibling + 1; page < totalPages; page++)
            {
                pages.Add(page);
            }
        }

        pages.Add(totalPages);

        return pages;
    }
}