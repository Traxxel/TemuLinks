# GitHub Release erstellen - Anleitung

## Schritt 1: Zu GitHub navigieren

1. Öffne deinen Browser und gehe zu: https://github.com/Traxxel/TemuLinks
2. Klicke auf **"Releases"** (rechts auf der Seite)
3. Klicke auf **"Create a new release"** (oder **"Draft a new release"**)

## Schritt 2: Release konfigurieren

### Tag auswählen:
- Wähle das Tag **v1.0.0** aus der Dropdown-Liste

### Release Title:
```
v1.0.0 - Browser Extensions für Chrome und Edge
```

### Release Description:
```markdown
## 🎉 Erste Release der TemuLinks Browser-Extensions

### ✨ Features
- 🌐 Chrome Extension für das Speichern von Temu-Links
- 🌐 Microsoft Edge Extension für das Speichern von Temu-Links
- 📌 One-Click-Button auf Temu.com-Seiten
- ⚙️ Konfigurierbare API-Einstellungen
- 🔒 API-Key-Authentifizierung
- 📝 Optionale Beschreibungen für Links
- 🌍 Öffentliche/Private Link-Optionen

### 📦 Installation

Siehe [PLUGIN_INSTALLATION.md](https://github.com/Traxxel/TemuLinks/blob/main/PLUGIN_INSTALLATION.md) für detaillierte Installationsanweisungen.

### 📥 Downloads

Wähle die passende Version für deinen Browser:
- **Chrome**: Lade `TemuLinks-Chrome-v1.0.0.zip` herunter
- **Edge**: Lade `TemuLinks-Edge-v1.0.0.zip` herunter

Nach dem Download entpacke die ZIP-Datei und lade die Extension als "Entpackte Erweiterung" in deinem Browser.
```

## Schritt 3: ZIP-Dateien hochladen

1. Scrolle nach unten zum Bereich **"Attach binaries by dropping them here or selecting them"**
2. Ziehe die beiden ZIP-Dateien in diesen Bereich (oder klicke und wähle sie aus):
   - `TemuLinks-Chrome-v1.0.0.zip` (aus dem Projektordner)
   - `TemuLinks-Edge-v1.0.0.zip` (aus dem Projektordner)

Die ZIP-Dateien befinden sich hier:
```
C:\Users\MeyerStefan\source\repos\Temulinks\TemuLinks-Chrome-v1.0.0.zip
C:\Users\MeyerStefan\source\repos\Temulinks\TemuLinks-Edge-v1.0.0.zip
```

## Schritt 4: Release veröffentlichen

1. Überprüfe alle Angaben
2. **Optional**: Setze das Häkchen bei "Set as the latest release"
3. Klicke auf **"Publish release"**

## Fertig! 🎉

Deine User können nun die Extensions herunterladen unter:
- https://github.com/Traxxel/TemuLinks/releases/latest

---

## Alternative: GitHub CLI verwenden

Falls du die GitHub CLI installiert hast, kannst du das Release auch per Kommandozeile erstellen:

```bash
gh release create v1.0.0 \
  TemuLinks-Chrome-v1.0.0.zip \
  TemuLinks-Edge-v1.0.0.zip \
  --title "v1.0.0 - Browser Extensions für Chrome und Edge" \
  --notes "Erste Release der TemuLinks Browser-Extensions mit Support für Chrome und Edge."
```


