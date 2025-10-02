# Anwendungsbeschreibung

Es sollen Anwendungen erstellt werden, welche miteinander arbeiten.
Sinn des Ganzen ist es, Weblinks der Seite https://temu.com wie in einer Wunschliste speichern zu können. Das Speichern soll über ein Browser-Plugin geschehen, wobei die aktuelle URL des Browsers verwendet wird, ein optionaler Beschreibungstext vom Anwender erfasst werden kann und ein boolscher Wert, ob dieser Link öffentlich sein soll.
Beim Klick auf Speichern im Browser-Plugin soll dieser Datensatz an die WebAPI gesendet werden, welche die entsprechenden CRUD-Befehle ausführt.

## Grobe Beschreibung

1. Eine C#-WebAPI, welche per Entity Framework Core mit einer MySQL-Datenbank zusammenarbeitet --> Web>API
2. Ein c# .net 8 DAL-Projekt, welches die Klassen für den Code-First im Entity Framework verwaltet und auf welches von allen Apps zugegriffen wird. --> DAL
3. Mehrere Browser-Plugins (zuerst nur Chrome, später MS Edge), welche mit der API sprechen. --> Plugins/Chrome
4. Ein Blazor-Webanwendung, in welcher sich Benutzer registrieren können, einen API-Key generieren und diesen in das Browser-Plugin eintragen können zur Authentifizierung. --> Web

## Detailbeschreibung

4. Anmelden / Registrieren

4.1 Registrieren / Anmelden
Der Anwender kann die Startseite aufrufen und soll sich unter Verwendung seines MS-Accounts registrieren können. Danach kann er sich aber noch nicht anmelden.

4.2 Benutzerrollen / Useraccounts
Es gibt 2 Benutzerrollen: Admin und User
Ein Admin kann über eine Benutzerverwaltung die Benutzer einsehen, editieren, aktivieren und sperren.
Ein Benutzer kann seine Benutzerdaten aktualisieren (Vorname, Nachname).
Beim Anmelden des allerersten Benutzers wird diesem automatisch die Rolle Admin zugewiesen und er soll automatisch freigeschaltet werden.

4.3 Listen
Da Temu-Einkauflisten geführt werden sollen sollen diese in der Webanwendung über ein Grid aufrufbar und editierbar sein. Ebenso soll der Anwender sie in der Sichtbarkeit für andere Anwender freigeben dürfen.

4.4 Öffentliche Listen
Alle Benutzer können öffentliche Links aller anderen Benutzer in einer gemeinsamen öffentlichen Liste einsehen.

3. Chrome-Plugin
   3.1 Konfiguration
   Es soll eine Konfiguration aufrufbar sein, in welcher der API-Endpunkt angegeben werden kann und der API-Schlüssel, welcher über die Webanwendung erzeugt werden kann.
   Wenn die aktuelle Seite mit https://temu.com beginnt, so soll ein Button speichern drückbar sein, mit welchem dieser Link plus eine optionale Beschreibung und die Freigabe (Checkbox) für andere Benutzer angegeben werden kann.
   Ebenso soll per API die Anzahl der gespeicherten Links aus der API abgeholt und angezeigt werden.
   Das Plugin soll einen Link zur Webanwendung bereitstellen, um die gespeicherten Links anzuzeigen.

## Technische Details

### Authentifizierung

- Webanwendung: Microsoft Account (MS-Account) Anmeldung
- API-Zugriff: API-Key basierte Authentifizierung
- Der API-Key wird in der Webanwendung generiert und im Browser-Plugin konfiguriert

### Datenmodell

Ein Temu-Link enthält:

- URL (automatisch von der aktuellen Browser-URL)
- Beschreibung (optional, vom Benutzer eingegeben)
- Öffentlich/Privat (boolean, Checkbox im Plugin)
- Benutzer (Verknüpfung über API-Key)
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

### API-Endpunkte

- GET /api/links (alle Links des authentifizierten Benutzers)
- POST /api/links (neuen Link erstellen)
- PUT /api/links/{id} (Link bearbeiten)
- DELETE /api/links/{id} (Link löschen)
- GET /api/links/public (alle öffentlichen Links aller Benutzer)
- GET /api/links/count (Anzahl der Links des Benutzers)

### Deployment

- Zunächst nur lokale Entwicklung
- Kein Docker
- Später eigenes Deployment
