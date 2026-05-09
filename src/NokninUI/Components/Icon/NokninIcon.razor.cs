using Microsoft.AspNetCore.Components;
using NokninUI.Data.Enums;

namespace NokninUI.Components.Icon;

public partial class NokninIcon
{
    [Parameter]
    public NokninIconName Name { get; set; }

    [Parameter]
    public NokninSize Size { get; set; } = NokninSize.Medium;

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private string IconClass
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Class))
            {
                return $"noknin-icon {SizeClass}";
            }

            return $"noknin-icon {SizeClass} {Class}";
        }
    }

    private string SizeClass => Size switch
    {
        NokninSize.Small => "noknin-icon--small",
        NokninSize.Medium => "noknin-icon--medium",
        NokninSize.Large => "noknin-icon--large",
        _ => "noknin-icon--medium"
    };

    private bool HasAccessibleName => !string.IsNullOrWhiteSpace(AriaLabel);
}
