# WebApp Dashboard

🇩🇪 [Deutsche Version](README-de.md)

A lightweight, borderless dashboard viewer for any web-based interface, built with .NET 10, WinForms and WebView2.

This repository is a **multi-brand setup**: the core application lives once in `WebAppDashboard/`; each brand is a thin variant in `variants/` that only overrides branding values (name, default page, default URL, single-instance key and icon). Publishing a new version builds every variant automatically.

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4) ![Platform](https://img.shields.io/badge/platform-Windows-0078D6) ![License](https://img.shields.io/badge/license-MIT-green)

---

## Variants

| Variant | Purpose |
|---|---|
| [`WebAppDashboard`](WebAppDashboard/) | Generic/template variant – default page "Dashboard", `http://localhost:8080/` |
| [`SymconDashboard`](variants/SymconDashboard/) | Branded for [IP-Symcon](https://www.symcon.de/) – default page "SYMCON", `http://localhost:3777/` |

Each brand already comes with its own `README.md`, `README-de.md` and `CHANGELOG.md` next to its `.csproj`.

---

## Features

- **Borderless window** – frameless display by default, ideal for dashboard use
- **Resizable & draggable** – resize from all edges, drag from the top strip
- **Edge snap** – window edges snap flush to the working-area boundary of any connected monitor when dragged or resized within 16 px; pull the cursor 32 px away from the edge to release
- **Drag bar window controls** – four caption buttons in the top strip: Kiosk · Minimize · Maximize/Restore · Close (exits the app). Clicking the Windows taskbar button toggles minimize/restore.
- **System tray integration** – right-click the tray icon for the full menu; double-click or re-launch to restore the window
- **Single-instance** – launching the app a second time restores the existing window (unique per brand)
- **Multiple pages** – configure any number of named pages (name + URL); switch instantly via the tray *Pages* submenu, the drag-bar list button (⊞), or the Page Manager dialog. The first entry is the startup page.
- **Page Manager** – dedicated dialog to add, rename, reorder and remove pages, including a per-page border color override (empty = global); *Apply* persists changes immediately without closing the dialog.
- **Taskbar icon toggle** – independently control whether the app appears in the Windows taskbar, regardless of borderless mode.
- **Configurable border color** – Windows accent color, auto-detected page background, custom hex color, or a per-page override for individual pages
- **Configurable border width** – presets or custom value (2–40 px)
- **Configurable zoom level** – presets (75–200 %) or custom value (25–500 %), saved across restarts
- **Kiosk mode** – one-click full-screen lockdown: `TopMost` + full current-screen bounds; toggle via drag bar button or tray menu
- **Persistent settings** – window position, size, URL and all preferences are saved automatically
- **First-run setup** – prompts for a URL on the first launch
- **Untrusted connections** – for local/intranet dashboards: `http` pages and `https` pages with invalid (e.g. self-signed) certificates load **without warnings** for the configured page hosts; login redirects and third-party hosts stay strictly validated
- **Error pages** – friendly error screens for HTTP and network failures
- **Localization** – English and German, automatically selected from Windows language settings

---

## Untrusted & Insecure Connections

This app is **designed for dashboards that run on plain `http` or use self-signed /
invalid TLS certificates** — typical for LAN devices (IP-Symcon, NAS, smart-home
gateways, etc.). Such pages are therefore handled **without a security warning**:

- **`http` pages (unencrypted)** are displayed without any warning.
- **`https` pages with invalid/untrusted certificates** are **accepted automatically**,
  but **only for the exact host + port of the pages configured in the Page Manager.**

**Scope:** The certificate exception applies solely to the matching host + port.
Redirects to other hosts (e.g. an external login provider), links to other servers
and embedded resources from third-party hosts are **validated strictly again** — the
usual certificate warning is shown there.

> **Security note:** This app is intended for **trusted local or intranet web
> front-ends only**. Data sent over `http` is **not encrypted**, and accepting an
> invalid certificate lets anyone who can impersonate the host intercept or modify
> the traffic. Run the app on trusted networks and use a proper HTTPS certificate
> for production systems.

---

## Requirements

| Component | Version |
|---|---|
| Windows | 10 or 11 |
| [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) | any (included with Windows 11 and Microsoft Edge) |

---

## Installation

1. Download the EXE of the brand you want from the [Releases](https://github.com/Apollo4244/WebAppDashboard/releases) page
2. **Move the EXE to a dedicated folder** (e.g. `C:\Tools\<Brand>\`) — do not run it directly from your Downloads folder
3. Run it — no installation required
4. On first launch, enter the URL of your web front-end

> **Tip:** The app creates additional files (`appsettings.json`, WebView2 cache) next to the EXE. Keeping it in its own folder prevents clutter in your Downloads folder.

No installer required.

---

## Usage

| Action | How |
|---|---|
| Open context menu | Right-click the tray icon **or** right-click the drag bar |
| Restore window | Double-click the tray icon **or** click the taskbar button **or** launch the app again |
| Minimize window | Drag bar `_` button **or** click the taskbar button |
| Switch page | Tray menu → *Pages* → page name **or** drag-bar ⊞ button |
| Manage pages | Tray menu → *Pages → Manage pages…* |
| Set per-page border color | Tray menu → *Pages → Manage pages…* → *Border color* field (empty = global) |
| Toggle borderless mode | Tray menu → *Borderless mode* (checkmark) |
| Change border color | Tray menu → *Borderless → Color* |
| Change border width | Tray menu → *Borderless → Width* |
| Change zoom level | Tray menu → *Zoom* **or** Ctrl+Plus / Ctrl+Minus (Ctrl+0 resets to configured level) |
| Toggle kiosk mode | Drag bar kiosk button **or** Tray menu → *Borderless → Kiosk mode* |
| Toggle taskbar icon | Tray menu → *Taskbar icon* (checkmark) |
| Reset window position | Tray menu → *Reset window position* |
| Exit | Close button (✕) in drag bar **or** Tray menu → *Exit* |

Settings are stored in `appsettings.json` next to the executable and are updated automatically.

---

## Command-line Options

| Option | Description |
|---|---|
| `--profile <name>` | Use `<name>.json` as the settings file and a separate WebView2 cache — enables multiple independent instances side by side |
| `--no-single-instance` | Skip the single-instance check entirely (no mutex); useful for scripted or testing scenarios |

Both options can be combined. Examples:

```
WebAppDashboard.exe --profile bedroom
WebAppDashboard.exe --profile kitchen
WebAppDashboard.exe --no-single-instance --profile debug
```

> **Note:** `--no-single-instance` without `--profile` shares the WebView2 cache between instances, which may cause the second instance to fail. Always pair it with `--profile` for reliable parallel use.

---

## Building from Source

**Prerequisites**

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows 10 or 11

**Clone and build**

```bash
git clone https://github.com/Apollo4244/WebAppDashboard.git
cd WebAppDashboard
dotnet build
```

**Publish all variants as single-file EXEs**

```powershell
pwsh .\scripts\build-all.ps1
```

Output goes to `publish/<BrandId>/`. To publish a single variant manually:

```bash
dotnet publish "variants/SymconDashboard/SymconDashboard.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish/SymconDashboard
```

**Run from source**

```bash
dotnet run --project WebAppDashboard
```

Or open `WebAppDashboard.slnx` in Visual Studio 2022 or later.

---

## Adding a New Brand

1. Copy the folder `variants/SymconDashboard/` to `variants/<YourBrand>Dashboard/`
2. Rename the `.csproj` accordingly and replace `app.ico` with the brand icon
3. Adjust the branding properties in the `.csproj` (all are optional — unset values fall back to `WebAppDashboard`):

   | Property | Meaning |
   |---|---|
   | `BrandId` | Identity / assembly name / root namespace (`<YourBrand>Dashboard`) |
   | `BrandDisplayName` | Title shown in the tray icon / title bar |
   | `BrandDefaultPageName` | Name of the first page on first run |
   | `BrandDefaultUrl` | Startup URL on first run |
   | `BrandMutexPrefix` | Basis of the single-instance mutex/event name |

4. Add your brand's `README.md`, `README-de.md` and `CHANGELOG.md`
5. Build with `.\scripts\build-all.ps1` — publish and workflow pick up new variants automatically

> The version for all variants is defined once in [`Directory.Build.props`](Directory.Build.props).

---

## Release Workflow

Pushing a tag `v*` triggers GitHub Actions, which publishes every variant and attaches all EXEs to the release:

```bash
git add . && git commit -m "Release v1.0.6"
git tag v1.0.6
git push origin main --tags
```

---

## License

MIT License – see [LICENSE](LICENSE) for details. The copyright notice must be retained in any copy or substantial portion of the software (one shared license for all variants).