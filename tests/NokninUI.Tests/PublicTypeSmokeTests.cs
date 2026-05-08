using NokninUI.Components.Button;
using NokninUI.Components.DataGrid;
using NokninUI.Components.Modal;
using NokninUI.Components.Pagination;
using NokninUI.Components.Select;
using NokninUI.Components.TextField;

namespace NokninUI.Tests;

public class PublicTypeSmokeTests
{
    [Fact]
    public void CorePublicTypes_AreReferenceable()
    {
        var types = new[]
        {
            typeof(NokninButton),
            typeof(NokninTextField),
            typeof(NokninSelect),
            typeof(NokninModal),
            typeof(NokninPagination),
            typeof(NokninDataGrid<>),
            typeof(NokninDataGridColumn<>),
            typeof(NokninSelectOption)
        };

        Assert.All(types, type => Assert.NotNull(type));
    }
}
