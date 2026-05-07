using Microsoft.AspNetCore.Components;

namespace NokninUI.Components.Table;

public partial class NokninTableHeader
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Class { get; set; }

    private string RootClass
    {
        get
        {
            return string.Join(
            " ",
            new[]
            {
                "noknin-table__header",
                Class
            }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
    }
}