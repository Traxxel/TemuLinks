# Anwendungsbeschreibung

Es sollen Anwendungen erstellt werden, welche miteinander arbeiten.
Sinn des Ganzen ist es, Weblinks der Seite https://temu.com wie in einer Wunschliste speichern zu können. Das Speichern soll über ein Browser-Plugin geschehen, wobei die aktuelle URL des Browsers verwendet wird, ein optionaler Beschreibungstext vom Anwender erfasst werden kann und ein boolscher Wert, ob dieser Link öffentlich sein soll.
Beim Klick auf Speichern im Browser-Plugin soll dieser Datensatz an die WebAPI gesendet werden, welche die entsprechenden CRUD-Befehle ausführt.

## Grobe Beschreibung

1. Eine C#-WebAPI, welche per Entity Framework Core mit einer MySQL-Datenbank zusammenarbeitet --> WebAPI
2. Ein c# .NET 8 DAL-Projekt, welches die Klassen für den Code-First im Entity Framework verwaltet und auf welches von allen Apps zugegriffen wird. --> DAL
3. Mehrere Browser-Plugins (zuerst nur Chrome, später MS Edge), welche mit der API sprechen. --> Plugins/Chrome
4. Eine Blazor WebAssembly App, in welcher sich Benutzer registrieren und anmelden können. --> WWW

## Detailbeschreibung

4. Anmelden / Registrieren

4.1 Registrieren / Anmelden
Der Anwender kann die Startseite aufrufen und sich mit Benutzername und Passwort registrieren. Die Anmeldedaten werden in der lokalen Datenbank gespeichert.

4.2 Benutzerrollen / Useraccounts
Es gibt 2 Benutzerrollen: Admin und User
Ein Admin kann über eine Benutzerverwaltung die Benutzer einsehen, editieren, aktivieren und sperren.
Ein Benutzer kann seine Benutzerdaten aktualisieren (Vorname, Nachname, Passwort).
Beim Registrieren des allerersten Benutzers wird diesem automatisch die Rolle Admin zugewiesen und er soll automatisch freigeschaltet werden.

4.3 Listen
Da Temu-Einkauflisten geführt werden sollen sollen diese in der Webanwendung über ein Grid aufrufbar und editierbar sein. Ebenso soll der Anwender sie in der Sichtbarkeit für andere Anwender freigeben dürfen.

4.4 Öffentliche Listen
Alle Benutzer können öffentliche Links aller anderen Benutzer in einer gemeinsamen öffentlichen Liste einsehen.

3. Chrome-Plugin
   3.1 Konfiguration
   Es soll eine Konfiguration aufrufbar sein, in welcher der API-Endpunkt angegeben werden kann und die Anmeldedaten (Benutzername/Passwort) gespeichert werden können.
   Wenn die aktuelle Seite mit https://temu.com beginnt, so soll ein Button speichern drückbar sein, mit welchem dieser Link plus eine optionale Beschreibung und die Freigabe (Checkbox) für andere Benutzer angegeben werden kann.
   Ebenso soll per API die Anzahl der gespeicherten Links aus der API abgeholt und angezeigt werden.
   Das Plugin soll einen Link zur Webanwendung bereitstellen, um die gespeicherten Links anzuzeigen.

## Technische Details

### Authentifizierung

- Webanwendung (WASM): Clientseitige Demo-Anmeldung (admin/admin) für Prototyp; später echte Anmeldung gegen WebAPI
- API-Zugriff: Token-basierte Authentifizierung (JWT) geplant
- Anmeldedaten werden im Browser-Plugin gespeichert (siehe Browser-Speicherung)

### Datenmodell

Ein Temu-Link enthält:

- URL (automatisch von der aktuellen Browser-URL)
- Beschreibung (optional, vom Benutzer eingegeben)
- Öffentlich/Privat (boolean, Checkbox im Plugin)
- Benutzer (Verknüpfung über UserId)
- Erstellungsdatum (automatisch)

### Datenbank

- MySQL Server lokal (localhost:3306)
- Benutzername: root
- Passwort: Geheim
- Datenbank: temulinks
- Konfiguration in appsettings.json

### Browser-Plugin

- Funktioniert nur auf https://temu.com
- Zeigt keine eigene Liste an
- Bietet Link zur Webanwendung für Listenansicht
- Speichert Anmeldedaten sicher im Browser (siehe Browser-Speicherung)

### Browser-Speicherung

Für die Speicherung der Anmeldedaten im Browser-Plugin stehen folgende Optionen zur Verfügung:

1. **Chrome Storage API (empfohlen)**

   - `chrome.storage.local` für lokale Speicherung
   - `chrome.storage.sync` für Synchronisation zwischen Geräten
   - Verschlüsselte Speicherung möglich
   - Automatische Bereinigung bei Extension-Deinstallation

2. **Web Storage (localStorage/sessionStorage)**

   - Einfacher zu implementieren
   - Weniger sicher (nicht verschlüsselt)
   - Persistiert auch nach Browser-Neustart

3. **IndexedDB**
   - Erweiterte Speichermöglichkeiten
   - Asynchrone API
   - Komplexere Implementierung

**Empfehlung**: Chrome Storage API mit `chrome.storage.local` für maximale Sicherheit und Benutzerfreundlichkeit.

### API-Endpunkte

- GET /api/links (alle Links des authentifizierten Benutzers)
- POST /api/links (neuen Link erstellen)
- PUT /api/links/{id} (Link bearbeiten)
- DELETE /api/links/{id} (Link löschen)
- GET /api/links/public (alle öffentlichen Links aller Benutzer)
- GET /api/links/count (Anzahl der Links des Benutzers)

### Deployment

- Zunächst nur lokale Entwicklung
- WebAPI: http://localhost:5002 (CORS erlaubt)
- Blazor WASM: http://localhost:5000
- Kein Docker (vorerst)
