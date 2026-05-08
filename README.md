# NokninUI
A semantic, themeable Blazor UI framework for modern applications.

## Status
NokninUI is currently in **early v0.1** and is a **work in progress**.  
You can use it today, but expect API and styling improvements as the library matures.

## Features
- Semantic design tokens (`--noknin-*`) for consistent theming
- Scoped component styling with extendable `Class` and `Style` parameters
- Light/dark theme support via `data-theme`
- Accessible-first primitives (labels, helper text, error states, focus-visible styles)
- Core UI building blocks for forms, feedback, overlays, navigation, and data display

## Installation
```bash
dotnet add package NokninUI
```

## Static Asset Setup
Add NokninUI styles to your host page (`index.html` for Blazor WebAssembly or `_Layout.cshtml` for Server):

```html
<link href="_content/NokninUI/css/noknin-ui.css" rel="stylesheet">
<link href="_content/NokninUI/NokninUI.styles.css" rel="stylesheet">
```

## Service Setup (Toasts)
Register the toast service in `Program.cs`:

```csharp
builder.Services.AddSingleton<NokninToastService>();
```

## Basic Usage
Import component namespaces in your app (for example in `_Imports.razor`) and use components directly.

### Button
```razor
<NokninButton Variant="NokninVariant.Primary">
    Save
</NokninButton>
```

### TextField
```razor
<NokninTextField Label="Email"
                 Placeholder="you@example.com"
                 @bind-Value="_email" />
```

### Select
```razor
<NokninSelect Label="Framework"
              Placeholder="Choose framework"
              Options="@_frameworkOptions"
              @bind-Value="_framework" />

@code {
    private string? _framework;

    private readonly IReadOnlyList<NokninSelectOption> _frameworkOptions =
    [
        new() { Value = "blazor", Label = "Blazor" },
        new() { Value = "react", Label = "React" },
        new() { Value = "vue", Label = "Vue" }
    ];
}
```

### Modal
```razor
<NokninButton Variant="NokninVariant.Primary" OnClick="@(() => _open = true)">
    Open modal
</NokninButton>

<NokninModal Open="@_open" OpenChanged="value => _open = value" Title="Confirm action">
    <ChildContent>
        Are you sure you want to continue?
    </ChildContent>
    <Footer>
        <NokninButton Variant="NokninVariant.Ghost" OnClick="@(() => _open = false)">Cancel</NokninButton>
        <NokninButton Variant="NokninVariant.Primary" OnClick="@(() => _open = false)">Confirm</NokninButton>
    </Footer>
</NokninModal>

@code {
    private bool _open;
}
```

### Toast
```razor
@inject NokninToastService Toasts

<NokninButton Variant="NokninVariant.Primary" OnClick="ShowToast">
    Show toast
</NokninButton>

<NokninToastContainer Position="NokninToastPosition.TopRight" />

@code {
    private void ShowToast()
    {
        Toasts.Success("Saved", "Your changes were saved successfully.");
    }
}
```

## Theming
NokninUI supports light/dark theme switching through a `data-theme` attribute:

```html
<div data-theme="light">
  ...
</div>

<div data-theme="dark">
  ...
</div>
```

Dark theme token overrides are applied when `data-theme="dark"` is present.

## Component List
- Accordion
- Alert
- Badge
- Button
- Card
- Checkbox
- DataGrid
- Dropdown
- Modal
- Pagination
- Radio
- Select
- Switch
- Table
- Tabs
- TextArea
- TextField
- Toast
- Tooltip

## Development Commands
```bash
dotnet restore
dotnet build NokninUI.slnx -v minimal
dotnet run --project src/NokninUI.Playground
dotnet pack src/NokninUI/NokninUI.csproj -c Release
```

## License
MIT. See [LICENSE](LICENSE).
