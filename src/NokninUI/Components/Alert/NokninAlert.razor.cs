using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Alert;

public partial class NokninAlert
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public NokninAlertVariant Variant { get; set; } = NokninAlertVariant.Info;

    [Parameter] public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter] public bool ShowIcon { get; set; } = true;

    [Parameter] public bool Dismissible { get; set; }

    [Parameter] public EventCallback OnDismiss { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public string? Role { get; set; }

    [Parameter] public string? AriaLive { get; set; }

    private NokninIconName IconName
    {
        get
        {
            return Variant switch
            {
                NokninAlertVariant.Success => NokninIconName.CircleCheck,
                NokninAlertVariant.Warning => NokninIconName.AlertTriangle,
                NokninAlertVariant.Error => NokninIconName.CircleAlert,
                NokninAlertVariant.Neutral => NokninIconName.Info,
                _ => NokninIconName.Info
            };
        }
    }

    private string ComputedRole
    {
        get
        {
            return Role ?? (Variant == NokninAlertVariant.Error ? "alert" : "status");
        }
    }

    private string ComputedAriaLive
    {
        get
        {
            return AriaLive ?? (Variant == NokninAlertVariant.Error ? "assertive" : "polite");
        }
    }

    private string ClassNames
    {
        get
        {
            return $"noknin-alert noknin-alert--{Variant.ToString().ToLowerInvariant()} noknin-alert--{Size.ToString().ToLowerInvariant()} {Class}".Trim();
        }
    }
}
