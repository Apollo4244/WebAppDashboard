# Changelog

All notable changes to this project (core) will be documented in this file.

Versions here refer to the shared version for all variants (see `Directory.Build.props`).
Full per-feature history of earlier releases can be found in the brand folders, e.g.
[`variants/SymconDashboard/CHANGELOG.md`](variants/SymconDashboard/CHANGELOG.md).

---

## [1.0.6] – Multi-brand refactor

### Added
- **Multi-brand solution:** The app now lives once in `WebAppDashboard/` (generic/template
  variant) and is re-branded through thin per-brand projects under `variants/`
  (`SymconDashboard`, …). Each variant only overrides branding values via its `.csproj`.
- **Branding via assembly metadata:** Brand values (`BrandId`, `BrandDisplayName`,
  `BrandDefaultPageName`, `BrandDefaultUrl`, `BrandMutexPrefix`) are embedded as
  `AssemblyMetadata` at build time and read at runtime by the shared `Brand` class.
  No code generation or duplication.
- **One global version:** `<Version>` is defined once in `Directory.Build.props` and
  applies to every variant.
- **Resource renaming:** Standalone resource base name is derived from the assembly name,
  so localized resources (`Strings.de`) follow each brand automatically without editing code.
- **Automatic build & release:** `scripts/build-all.ps1` and `.github/workflows/release.yml`
  publish every variant into `publish/<BrandId>/`; a `v*` tag creates a GitHub release with
  all EXEs. New variants are picked up automatically.

### Changed
- Namespaces renamed `SymconDashboard` → `WebAppDashboard`; per-variant assembly/root
  namespaces are now derived from `BrandId`.
- Default page name and startup URL are now brand values instead of hard-coded constants.
- `Form1.resx` (empty) and obsolete defaults removed.

### Removed
- `DlgPageDefault` string (replaced by `Brand.DefaultPageName`).
- Hard-coded default URLs (replaced by `Brand.DefaultUrl`).

---

## [1.0.5] and earlier

Prior releases covered the original single-brand history. See
[`variants/SymconDashboard/CHANGELOG.md`](variants/SymconDashboard/CHANGELOG.md) for details.