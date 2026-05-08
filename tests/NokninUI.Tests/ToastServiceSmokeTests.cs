using NokninUI.Data.Enums;
using NokninUI.Services;

namespace NokninUI.Tests;

public class ToastServiceSmokeTests
{
    [Fact]
    public void Show_RaisesOnShowWithExpectedPayload()
    {
        var service = new NokninToastService();
        NokninToastMessage? captured = null;

        service.OnShow += message => captured = message;

        service.Show("Saved", "Done", NokninToastVariant.Success, 2500);

        Assert.NotNull(captured);
        Assert.Equal("Saved", captured!.Title);
        Assert.Equal("Done", captured.Message);
        Assert.Equal(NokninToastVariant.Success, captured.Variant);
        Assert.Equal(2500, captured.Duration);
    }

    [Fact]
    public void Dismiss_RaisesOnDismissWithSameId()
    {
        var service = new NokninToastService();
        Guid? dismissedId = null;
        var id = Guid.NewGuid();

        service.OnDismiss += value => dismissedId = value;

        service.Dismiss(id);

        Assert.Equal(id, dismissedId);
    }
}
