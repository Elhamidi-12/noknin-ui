# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Ongoing improvements and stabilization work for the first public package release.

### Changed
- Ongoing API, accessibility, and release-readiness refinements.

### Fixed
- Ongoing bug fixes and polish before broader adoption.

## [0.1.0] - 2026-05-08

Initial prerelease for NokninUI.

### Added
- Core component set:
  - Button
  - TextField
  - TextArea
  - Select
  - Checkbox
  - Radio
  - Switch
  - Card
  - Badge
  - Alert
  - Modal
  - Dropdown
  - Tabs
  - Accordion
  - Tooltip
  - Toast
  - Table
  - Pagination
  - DataGrid
- Semantic token and theming foundation with light/dark support via `data-theme`.
- Blazor playground project for interactive examples and component validation.
- NuGet package metadata, README packaging support, and symbol package output (`.snupkg`).

### Changed
- Unified data component behavior and compatibility improvements:
  - DataGrid variant compatibility layer (`Variant`, with legacy boolean support).
  - Added explicit DataGrid loading/error/empty state ordering.
- Improved form accessibility and consistency:
  - Select listbox class alignment with CSS.
  - Optional `aria-labelledby` emission for Select and RadioGroup.
  - Added `Style` extensibility to Select and data primitives.

### Fixed
- Removed conflicting row roles in data components to preserve native table semantics.
- Replaced hardcoded destructive button color with semantic tokens.
- Added missing spacing token and destructive state token variants.
