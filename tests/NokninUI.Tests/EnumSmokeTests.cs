using NokninUI.Data.Enums;

namespace NokninUI.Tests;

public class EnumSmokeTests
{
    [Fact]
    public void NokninDataGridVariant_HasExpectedValues()
    {
        Assert.Equal(["Default", "Striped", "Bordered"], Enum.GetNames<NokninDataGridVariant>());
    }

    [Fact]
    public void NokninSize_HasExpectedValues()
    {
        Assert.Equal(["Small", "Medium", "Large"], Enum.GetNames<NokninSize>());
    }

    [Fact]
    public void NokninVariant_ContainsCoreButtonVariants()
    {
        var values = Enum.GetNames<NokninVariant>();

        Assert.Contains("Primary", values);
        Assert.Contains("Secondary", values);
        Assert.Contains("Ghost", values);
        Assert.Contains("Destructive", values);
    }
}
