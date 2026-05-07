using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Accordion;

public partial class NokninAccordionItem
{
    private readonly string _generatedValue = Guid.NewGuid().ToString("N");

    [CascadingParameter] public NokninAccordion? Accordion { get; set; }

    [Parameter] public string? Value { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }

    private string ItemValue
    {
        get
        {
            return Value ?? _generatedValue;
        }
    }

    private bool Open
    {
        get
        {
            return Accordion?.IsOpen(ItemValue) == true;
        }
    }

    private string TriggerId
    {
        get
        {
            return $"noknin-accordion-trigger-{ItemValue}";
        }
    }

    private string PanelId
    {
        get
        {
            return $"noknin-accordion-panel-{ItemValue}";
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-accordion-item {(Open ? "noknin-accordion-item--open" : "")} {(Disabled ? "noknin-accordion-item--disabled" : "")} {Class}".Trim();
        }
    }

    private void Toggle()
    {
        if (Disabled)
        {
            return;
        }

        Accordion?.Toggle(ItemValue);
    }
}