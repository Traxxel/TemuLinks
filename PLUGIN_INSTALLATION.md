# TemuLinks Browser-Extensions Installation

## Download

Lade die neueste Version der Browser-Extension herunter:

- **Chrome**: [TemuLinks-Chrome-v1.0.0.zip](https://github.com/Traxxel/TemuLinks/releases/latest/download/TemuLinks-Chrome-v1.0.0.zip)
- **Edge**: [TemuLinks-Edge-v1.0.0.zip](https://github.com/Traxxel/TemuLinks/releases/latest/download/TemuLinks-Edge-v1.0.0.zip)

## Installation

### Google Chrome

1. Lade die ZIP-Datei **TemuLinks-Chrome-v1.0.0.zip** herunter
2. Entpacke die ZIP-Datei in einen Ordner deiner Wahl
3. Öffne Chrome und navigiere zu `chrome://extensions/`
4. Aktiviere oben rechts den **"Entwicklermodus"**
5. Klicke auf **"Entpackte Erweiterung laden"**
6. Wähle den entpackten Ordner aus
7. Die Extension ist nun installiert! 🎉

### Microsoft Edge

1. Lade die ZIP-Datei **TemuLinks-Edge-v1.0.0.zip** herunter
2. Entpacke die ZIP-Datei in einen Ordner deiner Wahl
3. Öffne Edge und navigiere zu `edge://extensions/`
4. Aktiviere unten links den **"Entwicklermodus"**
5. Klicke auf **"Entpackt laden"**
6. Wähle den entpackten Ordner aus
7. Die Extension ist nun installiert! 🎉

## Konfiguration

Nach der Installation musst du die API-Einstellungen konfigurieren:

1. Klicke auf das **TemuLinks-Icon** in der Browser-Toolbar
2. Klicke auf **"API-Einstellungen konfigurieren"**
3. Gib folgende Informationen ein:
   - **API-Endpunkt**: Die URL deiner TemuLinks-API (z.B. `http://localhost:5002` oder `https://deine-domain.de`)
   - **API-Schlüssel**: Deinen persönlichen API-Schlüssel (generiere ihn in der TemuLinks-Webanwendung)
4. Klicke auf **"Konfiguration speichern"**
5. Teste die Verbindung mit dem Button **"Verbindung testen"**

## Verwendung

1. Besuche eine beliebige Seite auf **temu.com**
2. Klicke auf den Button **"📌 Save to TemuLinks"** (oben rechts auf der Seite)
   - ODER klicke auf das TemuLinks-Icon in der Browser-Toolbar
3. Gib optional eine Beschreibung ein
4. Wähle, ob der Link öffentlich sein soll
5. Klicke auf **"Link speichern"**

Fertig! Der Link ist nun in deiner TemuLinks-Collection gespeichert.

## Fehlerbehebung

### "Diese Erweiterung funktioniert nur auf Temu.com"
Die Extension funktioniert nur auf Seiten mit der Domain `temu.com`. Stelle sicher, dass du dich auf einer Temu-Seite befindest.

### "Bitte zuerst die API-Einstellungen konfigurieren"
Du musst zuerst deinen API-Endpunkt und API-Schlüssel in den Einstellungen hinterlegen.

### "Verbindung fehlgeschlagen"
- Überprüfe, ob deine TemuLinks-API läuft
- Überprüfe die URL (mit oder ohne Slash am Ende)
- Überprüfe deinen API-Schlüssel
- Überprüfe, ob CORS auf der API korrekt konfiguriert ist

## Support

Bei Problemen oder Fragen erstelle bitte ein [Issue auf GitHub](https://github.com/Traxxel/TemuLinks/issues).


