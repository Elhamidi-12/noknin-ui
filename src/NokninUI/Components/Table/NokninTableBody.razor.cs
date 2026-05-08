using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Table;

public partial class NokninTableBody
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Style { get; set; }

    private string RootClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-table__body",
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}
