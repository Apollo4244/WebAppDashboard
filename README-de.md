# WebApp Dashboard

🇬🇧 [English version](README.md)

Ein schlanker, rahmenloser Dashboard-Viewer für jede webbasierte Oberfläche, entwickelt mit .NET 10, WinForms und WebView2.

Dieses Repository ist ein **Multi-Brand-Aufbau**: Die Kern-App existiert einmal unter `WebAppDashboard/`; jede Marke ist eine dünne Variante unter `variants/`, die nur Branding-Werte überschreibt (Name, Startseite, Start-URL, Single-Instance-Schlüssel und Icon). Bei einer neuen Version werden automatisch alle Varianten gebaut.

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4) ![Platform](https://img.shields.io/badge/platform-Windows-0078D6) ![License](https://img.shields.io/badge/license-MIT-green)

---

## Varianten

| Variante | Zweck |
|---|---|
| [`WebAppDashboard`](WebAppDashboard/) | Generische/Template-Variante – Startseite „Dashboard", `http://localhost:8080/` |
| [`SymconDashboard`](variants/SymconDashboard/) | Gebrandet für [IP-Symcon](https://www.symcon.de/) – Startseite „SYMCON", `http://localhost:3777/` |

Jede Marke bringt ein eigenes `README.md`, `README-de.md` und `CHANGELOG.md` neben ihrem `.csproj` mit.

---

## Unsichere & nicht vertrauenswürdige Verbindungen

Diese App ist **für Dashboards gedacht, die über `http` laufen oder selbstsignierte /
ungültige TLS-Zertifikate verwenden** – typisch für LAN-Geräte (IP-Symcon, NAS,
Smart-Home-Gateways usw.). Solche Seiten werden deshalb **ohne Sicherheitswarnung**
angezeigt:

- **`http`-Seiten (unverschlüsselt)** werden ohne Warnung dargestellt.
- **`https`-Seiten mit ungültigen/nicht vertrauenswürdigen Zertifikaten** werden
  **automatisch akzeptiert**, aber **nur für exakt den Host + Port der im
  Seiten-Manager konfigurierten Seiten.**

**Gültigkeitsbereich:** Die Zertifikats-Ausnahme gilt ausschließlich für den
passenden Host + Port. Weiterleitungen auf andere Hosts (z. B. zu einem externen
Login-Anbieter), Links zu anderen Servern und eingebettete Ressourcen Dritter werden
**weiterhin streng validiert** – dort erscheint die übliche Zertifikats-Warnung.

> **Sicherheitshinweis:** Die App ist **ausschließlich für vertrauenswürdige lokale
> oder Intranet-Weboberflächen gedacht**. Über `http` übertragene Daten sind
> **nicht verschlüsselt**, und ein akzeptiertes ungültiges Zertifikat erlaubt jedem,
> der den Host imitiert, den Verkehr abzuhören oder zu verändern. Die App nur in
> vertrauenswürdigen Netzen betreiben und für Produktivsysteme ein echtes
> HTTPS-Zertifikat verwenden.

---

## Funktionen

- **Rahmenloses Fenster** – standardmäßig ohne Titelleiste, ideal für Dashboard-Einsatz
- **Größenänderung & Verschieben** – Größe an allen Kanten anpassbar, Ziehen über den oberen Streifen
- **Kantenfang** – beim Verschieben oder Skalieren rasten Fensterkanten automatisch an den WorkingArea-Grenzen jedes angeschlossenen Monitors ein, sobald sie sich auf unter 16 px annähern; 32 px vom Rand wegziehen zum Lösen
- **Drag-Leisten-Schaltflächen** – vier Steuerschaltflächen im oberen Streifen: Kiosk · Minimieren · Maximieren/Wiederherstellen · Schließen (beendet die App). Ein Klick auf das Taskleistensymbol wechselt zwischen Minimieren und Wiederherstellen.
- **System-Tray-Integration** – Rechtsklick auf das Tray-Icon öffnet das vollständige Menü; Doppelklick oder Neustart stellt das Fenster wieder her
- **Einzelinstanz** – ein erneuter Programmstart stellt das bereits laufende Fenster in den Vordergrund (pro Marke eindeutig)
- **Mehrere Seiten** – beliebig viele benannte Seiten (Name + URL) konfigurierbar; Umschalten über das Tray-Untermenü *Seiten*, die ⊞-Schaltfläche in der Drag-Leiste oder den Seiten-Manager. Der erste Eintrag ist die Startseite.
- **Seiten-Manager** – eigener Dialog zum Hinzufügen, Umbenennen, Sortieren und Entfernen von Seiten, inkl. eigener Rahmenfarbe pro Seite (leer = global); *Übernehmen* speichert sofort, ohne den Dialog zu schließen.
- **Taskleisten-Symbol** – unabhängig vom rahmenlosen Modus steuerbar, ob die App in der Windows-Taskleiste erscheint.
- **Konfigurierbare Rahmenfarbe** – Windows-Akzentfarbe, automatisch von der Seitenhintergrundfarbe erkannt, benutzerdefinierte Hex-Farbe oder eine pro Seite gesetzte Farbe
- **Konfigurierbare Rahmenbreite** – Voreinstellungen oder eigener Wert (2–40 px)
- **Konfigurierbarer Zoom** – Voreinstellungen (75–200 %) oder eigener Wert (25–500 %), wird beim Neustart wiederhergestellt
- **Kiosk-Modus** – Vollbild-Sperrung per Klick: `TopMost` + voller aktueller Monitor; umschalten über Drag-Leisten-Schaltfläche oder Tray-Menü
- **Einstellungen werden gespeichert** – Fensterposition, -größe, URL und alle Optionen werden automatisch gesichert
- **Erststart-Einrichtung** – beim ersten Start wird nach einer URL gefragt
- **Unsichere Verbindungen** – für lokale/Intranet-Dashboards: `http`-Seiten und `https`-Seiten mit ungültigen (z. B. selbstsignierten) Zertifikaten laden für die konfigurierten Seiten-Hosts **ohne Warnung**; Login-Weiterleitungen und Fremd-Hosts bleiben streng validiert
- **Fehlerseiten** – übersichtliche Fehlerseiten bei HTTP- und Netzwerkfehlern
- **Lokalisierung** – Englisch und Deutsch, automatisch anhand der Windows-Spracheinstellung gewählt

---

## Voraussetzungen

| Komponente | Version |
|---|---|
| Windows | 10 oder 11 |
| [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) | beliebig (in Windows 11 und Microsoft Edge enthalten) |

---

## Installation

1. Die EXE der gewünschten Marke von der [Releases](https://github.com/Apollo4244/WebAppDashboard/releases)-Seite herunterladen
2. **Die EXE in einen eigenen Ordner verschieben** (z. B. `C:\Tools\<Marke>\`) — nicht direkt aus dem Download-Ordner ausführen
3. Starten — keine Installation erforderlich
4. Beim ersten Start die URL der Weboberfläche eingeben

> **Hinweis:** Die App legt zusätzliche Dateien (`appsettings.json`, WebView2-Cache) neben der EXE ab. Ein eigener Ordner verhindert Unordnung im Download-Ordner.

Kein Installer erforderlich.

---

## Bedienung

| Aktion | So geht's |
|---|---|
| Kontextmenü öffnen | Rechtsklick auf das Tray-Icon **oder** Rechtsklick auf den Drag-Streifen |
| Fenster wiederherstellen | Doppelklick auf das Tray-Icon **oder** Taskleistensymbol anklicken **oder** Programm erneut starten |
| Fenster minimieren | `_`-Schaltfläche in der Drag-Leiste **oder** Taskleistensymbol anklicken |
| Seite wechseln | Tray-Menü → *Seiten* → Seitenname **oder** ⊞-Schaltfläche in der Drag-Leiste |
| Seiten verwalten | Tray-Menü → *Seiten → Seiten verwalten…* |
| Rahmenfarbe pro Seite setzen | Tray-Menü → *Seiten → Seiten verwalten…* → Feld *Rahmenfarbe* (leer = global) |
| Rahmenlosen Modus umschalten | Tray-Menü → *Rahmenloser Modus* (Häkchen) |
| Rahmenfarbe ändern | Tray-Menü → *Rahmenlos → Farbe* |
| Rahmenbreite ändern | Tray-Menü → *Rahmenlos → Breite* |
| Zoom ändern | Tray-Menü → *Zoom* **oder** Strg+Plus / Strg+Minus (Strg+0 setzt auf konfigurierten Zoom zurück) |
| Kiosk-Modus umschalten | Kiosk-Schaltfläche in der Drag-Leiste **oder** Tray-Menü → *Rahmenlos → Kiosk-Modus* |
| Taskleisten-Symbol umschalten | Tray-Menü → *Taskleisten-Symbol* (Häkchen) |
| Fensterposition zurücksetzen | Tray-Menü → *Fensterposition zurücksetzen* |
| Beenden | Schließen-Schaltfläche (✕) in der Drag-Leiste **oder** Tray-Menü → *Beenden* |

Die Einstellungen werden in `appsettings.json` neben der ausführbaren Datei gespeichert und automatisch aktualisiert.

---

## Kommandozeilenoptionen

| Option | Beschreibung |
|---|---|
| `--profile <Name>` | Verwendet `<Name>.json` als Einstellungsdatei und einen eigenen WebView2-Cache – ermöglicht mehrere unabhängige Instanzen gleichzeitig |
| `--no-single-instance` | Übergeht den Single-Instance-Check (kein Mutex); nützlich für Skripte oder Testzwecke |

Beide Optionen können kombiniert werden. Beispiele:

```
WebAppDashboard.exe --profile schlafzimmer
WebAppDashboard.exe --profile küche
WebAppDashboard.exe --no-single-instance --profile debug
```

> **Hinweis:** `--no-single-instance` ohne `--profile` teilt den WebView2-Cache zwischen den Instanzen, was zum Fehler der zweiten Instanz führen kann. Für zuverlässigen Parallelbetrieb immer mit `--profile` kombinieren.

---

## Aus dem Quellcode bauen

**Voraussetzungen**

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows 10 oder 11

**Klonen und bauen**

```bash
git clone https://github.com/Apollo4244/WebAppDashboard.git
cd WebAppDashboard
dotnet build
```

**Alle Varianten als Single-File-EXEs publishen**

```powershell
pwsh .\scripts\build-all.ps1
```

Das Ergebnis liegt unter `publish/<BrandId>/`. Manuell eine einzelne Variante publishen:

```bash
dotnet publish "variants/SymconDashboard/SymconDashboard.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish/SymconDashboard
```

**Aus dem Quellcode starten**

```bash
dotnet run --project WebAppDashboard
```

Oder `WebAppDashboard.slnx` in Visual Studio 2022 oder neuer öffnen.

---

## Neue Marke hinzufügen

1. Den Ordner `variants/SymconDashboard/` zu `variants/<Ihre Marke>Dashboard/` kopieren
2. Die `.csproj` entsprechend umbenennen und `app.ico` durch das Marken-Icon ersetzen
3. Die Branding-Eigenschaften im `.csproj` anpassen (alle optional – nicht gesetzte Werte fallen auf `WebAppDashboard` zurück):

   | Eigenschaft | Bedeutung |
   |---|---|
   | `BrandId` | Identität / Assemblyname / Root-Namespace (`<Ihre Marke>Dashboard`) |
   | `BrandDisplayName` | Titel im Tray / in der Titelleiste |
   | `BrandDefaultPageName` | Name der ersten Seite beim Erststart |
   | `BrandDefaultUrl` | Start-URL beim Erststart |
   | `BrandMutexPrefix` | Basis der Single-Instance-Mutex-/Eventnamen |

4. Eigenes `README.md`, `README-de.md` und `CHANGELOG.md` für die Marke anlegen
5. Mit `.\scripts\build-all.ps1` bauen – Publish-Skript und CI übernehmen neue Varianten automatisch

> Die Version aller Varianten ist zentral einmal in [`Directory.Build.props`](Directory.Build.props) definiert.

---

## Release-Workflow

Ein Tag `v*` löst GitHub Actions aus, das alle Varianten publisht und alle EXEs anhängt:

```bash
git add . && git commit -m "Release v1.0.6"
git tag v1.0.6
git push origin main --tags
```

---

## Lizenz

MIT-Lizenz – Details siehe [LICENSE](LICENSE). Der Copyright-Hinweis muss bei jeder Weitergabe des Programms oder wesentlicher Teile erhalten bleiben (eine gemeinsame Lizenz für alle Varianten).