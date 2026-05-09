using NokninUI.Components.Icon;
using NokninUI.Data.Enums;

namespace NokninUI.Tests;

public class IconSmokeTests
{
    [Fact]
    public void NokninIcon_PublicTypes_AreReferenceable()
    {
        Assert.NotNull(typeof(NokninIcon));
        Assert.NotNull(typeof(NokninIconName));
    }

    [Fact]
    public void NokninIconName_HasExpectedValues()
    {
        var values = Enum.GetNames<NokninIconName>();

        Assert.Contains("Menu", values);
        Assert.Contains("Sun", values);
        Assert.Contains("Moon", values);
        Assert.Contains("Check", values);
        Assert.Contains("X", values);
        Assert.Contains("ChevronDown", values);
        Assert.Contains("MoreHorizontal", values);
        Assert.Contains("CircleCheck", values);
        Assert.Contains("CircleAlert", values);
    }
}
