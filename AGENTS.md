# AGENTS.md

Instructions for AI agents (and developers) working on this repository.

## Project Overview

WebApp Dashboard is a **multi-brand WinForms app** (.NET 10, WinForms, WebView2)
with a lightweight, borderless dashboard viewer. The application code exists **exactly once**
in the core project; each brand is a thin variant that only overrides branding values.

```
WebAppDashboard.slnx            Solution containing all projects
Directory.Build.props           Global version (single source for all variants)
LICENSE                         One MIT license (copyright Apollo4244) for all variants – the notice must not be removed
scripts/build-all.ps1           Local publish loop over all variants
.github/workflows/release.yml   CI: publishes all variants on tag v*, creates GitHub release
WebAppDashboard/                Core project = generic (template) variant
  WebAppDashboard.csproj        Core project file (BrandId = WebAppDashboard)
  WebAppDashboard.brand.props   Shared build settings + branding defaults
  WebAppDashboard.brand.targets Shared item globs, resources, PackageReference, AssemblyMetadata
  Brand.cs                      Reads branding values at runtime from assembly metadata
  *.cs, *.resx                  Application sources (namespace WebAppDashboard)
variants/<Brand>Dashboard/      One brand per folder (e.g. SymconDashboard/)
  <Brand>Dashboard.csproj       Only sets BrandId + overridden branding values + icon
  app.ico                       Brand icon
  README.md, README-de.md, CHANGELOG.md   Brand documentation
publish/<BrandId>/              Output of build-all.ps1 / CI (one single-file EXE per variant)
```

## Important Commands

```powershell
dotnet build WebAppDashboard.slnx              # build everything (0 errors expected)
pwsh .\scripts\build-all.ps1                   # publish all variants to single-file EXEs
```

Publish a single variant:

```powershell
dotnet publish WebAppDashboard/WebAppDashboard.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish/<BrandId>
```

- The publish flags matter: `--self-contained true` (runtime embedded),
  `PublishSingleFile=true` (one EXE), `IncludeNativeLibrariesForSelfExtract=true` (WebView2 loader inside the EXE).
- As in the original project, the publish output is a single EXE; `bin/`/`obj/` are only intermediate build outputs and never part of a release.

### MSB3277 (WindowsBase 5.0.0.0 vs 4.0.0.0) – handled, not a bug

The WebView2 package unconditionally references `Microsoft.Web.WebView2.Wpf.dll`
(compiled against .NET 5 WPF, i.e. `WindowsBase 5.0.0.0`) for every net5.0+ project,
even WinForms-only ones. WinForms-only apps get the `WindowsBase 4.0.0.0` facade from
`Microsoft.NETCore.App` instead, so MSBuild reports MSB3277. It is compile-time only
(the WPF DLL is never used/loaded) and also occurred in the original project.

This is handled centrally: `WebAppDashboard.brand.targets` removes the unused WPF
reference via the `RemoveUnusedWebView2Wpf` target, guarded by `UseWPF`. Therefore
no action is needed and the warning must **not** reappear in builds.

- Variants that deliberately use WPF set `UseWPF=true` – the target then keeps the
  reference and the conflict resolves itself (real `WindowsBase` from WindowsDesktop.App).
- When bumping the WebView2 package, run `dotnet build WebAppDashboard.slnx` and confirm
  MSB3277 stays gone (the package bug persists in recent versions; the removal is a
  harmless no-op once Microsoft fixes it).

## Versioning

- `<Version>` lives **only** in `Directory.Build.props` and applies globally to all variants.
- Bump it exactly there before a release – nowhere else. There are no per-`.csproj` versions.

## How Branding Works

1. The variant sets a minimal `BrandId` (near the top of its `.csproj`).
2. `brand.props` is imported first (after that the variant can override values),
   `brand.targets` last (reads the final values).
3. `brand.targets` emits `<AssemblyMetadata Include="..." Value="..." />` for `BrandId`,
   `BrandDisplayName`, `BrandDefaultPageName`, `BrandDefaultUrl`, `BrandMutexPrefix`.
4. `Brand.cs` reads these metadata at runtime (namespace `WebAppDashboard`).
   **When adding a new variant you must not change `Brand.cs` or any sources.**

    Unset branding properties fall back in `brand.props` to the generic
    `WebAppDashboard` values (DisplayName "WebApp Dashboard", page "Dashboard",
    URL `http://localhost:8080/`, mutex `WebApp-Dashboard`).

### Resources / Localization

- `Strings.resx` (en) and `Strings.de.resx` (de) live only in the core and are embedded
  via `brand.targets` as `$(AssemblyName).Strings.resources` and `$(AssemblyName).Strings.de.resources`.
  The ResourceManager base name is derived at runtime from the assembly name.
  → Nothing to do for a new variant; the satellites move into the single-file EXE automatically.

### Single-Instance (watch out for update compatibility)

- The mutex/event names are based on `BrandMutexPrefix` (Program.cs, suffix `41E2C7F3` or `41E2C7F3-<profile>`).
- **Never change existing `BrandMutexPrefix` values** (e.g. `Symcon-Dashboard-for-Windows`),
  otherwise the app may share data with installed versions or open a second instance.
- For a new brand, assign a unique new prefix.

## Adding a New Variant – Complete Checklist

Starting point: a 1:1 copy of `variants/SymconDashboard/`.

1. **Create and copy the folder**
   Copy `variants/SymconDashboard/` → `variants/<NewBrand>Dashboard/`
   (csproj, `app.ico`, README, README-de, CHANGELOG).

2. **Rename the `.csproj`**
   `SymconDashboard.csproj` → `<NewBrand>Dashboard.csproj`
   (Important: no dots in the name – otherwise you get e.g. `<Brand>.Dashboard.exe`).
   The file name becomes `<BrandId>.exe` when publishing.

3. **Adjust the `.csproj` contents** (see `SymconDashboard.csproj` as template):

   | Property | Meaning | Example |
   |---|---|---|
   | `BrandId` | Identity/AssemblyName/RootNamespace – no dots | `NewBrandDashboard` |
   | `BrandDisplayName` | Tray/title-bar title (human-readable, spaces allowed) | `New Brand Dashboard` |
   | `BrandDefaultPageName` | Name of the first page on first run | `START` |
   | `BrandDefaultUrl` | Startup URL on first run | `http://localhost:8080/` |
   | `BrandMutexPrefix` | Unique single-instance key | `NewBrand-Dashboard` |
   | `ApplicationIcon` | stays `$(MSBuildThisFileDirectory)app.ico` | – |

   Leave the imports (`brand.props`/`brand.targets`) unchanged – they point at the core project.

4. **Replace `app.ico`** with the brand icon (same size ideals: 32 + 256 px).

5. **Create/update the brand documentation** in `variants/<NewBrand>Dashboard/`, covering:
   - `README.md` / `README-de.md`: title, release link, example URL, `--profile` examples using the new EXE name, build section (repo `WebAppDashboard`).
   - `CHANGELOG.md`: the brand's version history; start at 1.0.0 and note that the base-app history lives in the core `CHANGELOG.md`/Symcon variant (or carry it over).

6. **Update the root documentation**:
   - `README.md` **and** `README-de.md` (always both!):
     - Add the new brand to the variants table.
     - Keep the "Add a new brand" instructions consistent if needed.
   - `CHANGELOG.md` (core): mention the new variant under an appropriate entry.

7. **Script & GitHub Actions – usually NO change needed**:
   - `scripts/build-all.ps1` and `.github/workflows/release.yml` automatically publish all
     `variants/*/*.csproj` (glob) to `publish/<BrandId>/`, and CI attaches `publish/**/*.exe` to the release.
   - **Only if** a variant deviates from the folder/naming convention (different granularity, multiple csproj
     per folder, proper names containing dots), update both files accordingly and check the core list.
   - Update `.slnx`: add the new variant as `<Project Path="variants/<NewBrand>Dashboard/<NewBrand>Dashboard.csproj" />`.

8. **Verification (mandatory)**
   ```powershell
   dotnet build WebAppDashboard.slnx                          # 0 errors
   pwsh .\scripts\build-all.ps1                               # publish all variants
   # Check the result:
   Get-ChildItem publish -Filter *.exe -Recurse | Select-Object FullName, Length
   ```
   Expected: `publish/<NewBrand>Dashboard/<NewBrand>Dashboard.exe` (a single file).
   Optionally check the satellites inside the EXE (byte search for `<BrandId>.Strings.de.resources`).

## Release Workflow

1. Bump `<Version>` in `Directory.Build.props`.
2. `git add . && git commit -m "Release vX.Y.Z"`
3. `git tag vX.Y.Z && git push origin main --tags`
4. GitHub Actions publishes all variants and creates the release (all `*.exe` as assets).

## DPI / Windows Scaling (WinForms)

The app is DPI-aware (the OS scales fonts on its own), but **`AutoScaleMode.Font`
does not scale hand-built forms** – the programmatic forms do not use the designer
pattern, so auto-scaling never takes effect there. All layouts therefore work
with real font metrics at runtime:

- **Derive text-bearing heights from `Font.Height`** – never hard-code pixel heights.
  Otherwise descenders (y, p, g, …) get clipped at 125–200 % scaling.
  - Label: `Height = Font.Height`
  - Single-line TextBox: `Height = Font.Height + 6`
  - Buttons: `Height = Font.Height + 19` (small) / `Font.Height + 25` (large)
- Build rows from these heights with a uniform label→field gap (~10 px) and
  row gap (~14 px) so all fields stay identically spaced at every DPI.
- The borderless window uses `DragBarHeight = Max(BorderSize, Font.Height + 8)` so
  the page label and caption icons fit at any scaling; text labels inherit the
  form's default font instead of creating a fixed-size `Font`.
- Give dialogs a **fixed `ClientSize`** (unscaled); only the text-sized controls
  scale themselves. Do not apply a global size multiplier – that would push
  dialogs off small screens running at high DPI.
- Do not (re-)introduce `AutoScaleMode`/`AutoScaleDimensions`; follow this scheme
  when adding controls anywhere in the app.

## Conventions

- Folder `scripts/` in lowercase; build scripts go to `scripts/`.
- `BrandId`/assembly/file names without dots (`WebAppDashboard`, `SymconDashboard`, `<Brand>Dashboard`).
- Source namespace: `WebAppDashboard` (never changed).
- Do not duplicate core sources – everything lives in `WebAppDashboard/` and is pulled in via `brand.targets`.
- Do not add comments in code unless the user asks for them.
- Do not commit secrets/keys (`bin/`, `obj/`, `publish/` are excluded via `.gitignore` – outputs there never end up in a commit).
- License: do not change or remove the copyright notice in `LICENSE`.