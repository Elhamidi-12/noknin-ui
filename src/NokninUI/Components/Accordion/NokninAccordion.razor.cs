using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Accordion;

public partial class NokninAccordion
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Multiple { get; set; }
    [Parameter] public string? Class { get; set; }

    private readonly HashSet<string> _openItems = [];

    internal bool IsOpen(string value)
    {
        return _openItems.Contains(value);
    }

    internal void Toggle(string value)
    {
        if (_openItems.Contains(value))
        {
            _openItems.Remove(value);
            return;
        }

        if (!Multiple)
        {
            _openItems.Clear();
        }

        _openItems.Add(value);
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-accordion {Class}".Trim();
        }
    }
}