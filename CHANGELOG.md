# Changelog

All notable changes to this project (core) will be documented in this file.

Versions here refer to the shared version for all variants (see `Directory.Build.props`).
Full per-feature history of earlier releases can be found in the brand folders, e.g.
[`variants/SymconDashboard/CHANGELOG.md`](variants/SymconDashboard/CHANGELOG.md).

---

## [Unreleased]

### Added
- **Invalid TLS certificates are accepted for configured pages:** `https` pages
  with invalid/untrusted certificates (e.g. self-signed) load **without a warning**
  as long as the host + port matches a page configured in the Page Manager; plain
  `http` pages are unaffected. Every other host (login redirects, external links,
  embedded resources) stays strictly validated and shows the usual certificate
  warning.

### Fixed
- **Text clipped at high Windows scaling (125–200 %):** Labels, input fields and
  buttons now derive their heights from the actual font height (`Font.Height`)
  instead of fixed pixel values, so descenders (`y`, `p`, `g`, …) are no longer
  cut off – in the Page Manager, the custom color/width/zoom dialogs and the
  borderless drag-bar title. `AutoScaleMode.Font` is not used; layouts are built
  from real font metrics at runtime.

---

## [1.0.7]

### Added
- **Per-page border color:** Each page in the *Manage pages* dialog can define
  its own border color (`#RRGGBB`), overriding the global border color for that
  page only. An empty field falls back to the global setting.

### Fixed
- **Auto border color applies without manual refresh:** The detected page
  background color is cached per page, so the matching border color is applied
  immediately when switching pages; switching the global mode to *auto* starts
  detection right away.
- **Invalid custom colors can no longer be saved:** Hex input in the tray menu
  and the Page Manager accepts only `#RRGGBB`; 8-digit alpha colors
  (`#AARRGGBB`), shorter or otherwise malformed values are rejected with a
  warning.

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