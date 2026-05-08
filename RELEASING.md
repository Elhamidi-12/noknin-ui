# Releasing NokninUI

This document describes the manual release flow for NokninUI until automated publishing is introduced.

## 1. Update Version
- Update the package version in `src/NokninUI/NokninUI.csproj` (for example `VersionPrefix`).

## 2. Update Changelog
- Add a new release section in `CHANGELOG.md`.
- Move relevant items from `Unreleased` into the new version section.

## 3. Build
```bash
dotnet build NokninUI.slnx -c Release -v minimal
```

## 4. Pack
```bash
dotnet pack src/NokninUI/NokninUI.csproj -c Release --no-build
```

## 5. Verify Package Locally
- Inspect generated artifacts under:
  - `src/NokninUI/bin/Release/NokninUI.<version>.nupkg`
  - `src/NokninUI/bin/Release/NokninUI.<version>.snupkg`
- Validate package metadata and README rendering.
- Optionally install the package in a sample app to verify runtime/static assets.

### Local `.nupkg` Verification (Recommended Before Publish)
Use this flow to validate the package as a real consumer would install it.

1. Build and pack in Release mode:
```bash
dotnet build NokninUI.slnx -c Release -v minimal
dotnet pack src/NokninUI/NokninUI.csproj -c Release --no-build
```

2. Create a temporary local NuGet source folder:
```bash
mkdir .artifacts/local-nuget
```

3. Copy the generated package into that source:
```bash
cp src/NokninUI/bin/Release/NokninUI.0.1.0.nupkg .artifacts/local-nuget/
```

4. Create a fresh Blazor test app in a temp folder:
```bash
dotnet new blazorwasm -n NokninUI.LocalSmoke
cd NokninUI.LocalSmoke
```

5. Add the local NuGet source:
```bash
dotnet nuget add source ../.artifacts/local-nuget --name NokninLocal
```

6. Install NokninUI from the local source:
```bash
dotnet add package NokninUI --source ../.artifacts/local-nuget
```

7. Add required CSS links in `wwwroot/index.html`:
```html
<link href="_content/NokninUI/css/noknin-ui.css" rel="stylesheet">
<link href="_content/NokninUI/NokninUI.styles.css" rel="stylesheet">
```

8. Register toast service in `Program.cs`:
```csharp
builder.Services.AddSingleton<NokninToastService>();
```

9. Render a simple Noknin button (for example in `Pages/Index.razor`):
```razor
<NokninButton Variant="NokninVariant.Primary">
    Local package smoke test
</NokninButton>
```

10. Run the app and verify component styling:
```bash
dotnet run
```

## 6. Tag Release
- Create and push a git tag for the release version (for example `v0.1.0`).

## 7. Publish (Manual, Later)
- Publish to NuGet manually when ready.
- Automated publish workflow is intentionally out of scope for now.
