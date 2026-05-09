using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;
using NokninUI.Services;

namespace NokninUI.Components.Toast;

public partial class NokninToast
{
    [Parameter, EditorRequired] public NokninToastMessage Message { get; set; } = default!;
    [Parameter] public EventCallback<Guid> OnDismiss { get; set; }

    private string ClassNames
    {
        get
        {
            return $"noknin-toast noknin-toast--{Message.Variant.ToString().ToLowerInvariant()}";
        }
    }

    private string Role
    {
        get
        {
            return Message.Variant == NokninToastVariant.Error ? "alert" : "status";
        }
    }

    private string AriaLive
    {
        get
        {
            return Message.Variant == NokninToastVariant.Error ? "assertive" : "polite";
        }
    }

    private NokninIconName IconName
    {
        get
        {
            return Message.Variant switch
            {
                NokninToastVariant.Success => NokninIconName.CircleCheck,
                NokninToastVariant.Warning => NokninIconName.AlertTriangle,
                NokninToastVariant.Error => NokninIconName.CircleAlert,
                NokninToastVariant.Neutral => NokninIconName.Info,
                _ => NokninIconName.Info
            };
        }
    }

    private async Task Dismiss()
    {
        await OnDismiss.InvokeAsync(Message.Id);
    }
}
