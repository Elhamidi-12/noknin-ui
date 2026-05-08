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

## 6. Tag Release
- Create and push a git tag for the release version (for example `v0.1.0`).

## 7. Publish (Manual, Later)
- Publish to NuGet manually when ready.
- Automated publish workflow is intentionally out of scope for now.
