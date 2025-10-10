# 🔗 TemuLinks

Eine vollständige Lösung zum Speichern und Verwalten von Temu.com-Links als persönliche Wunschliste. Das Projekt besteht aus einer WebAPI, einer Blazor WebAssembly-Anwendung, Browser-Extensions und einem gemeinsamen Data Access Layer.

## 📋 Inhaltsverzeichnis

- [Übersicht](#-übersicht)
- [Komponenten](#-komponenten)
  - [WWW (Webanwendung)](#-www-webanwendung)
  - [WebAPI](#-webapi)
  - [DAL (Data Access Layer)](#-dal-data-access-layer)
  - [Browser-Plugins](#-browser-plugins)
- [Features](#-features)
- [Technologie-Stack](#-technologie-stack)
- [Architektur](#-architektur)
- [Installation & Setup](#-installation--setup)
- [Verwendung](#-verwendung)
- [API-Endpunkte](#-api-endpunkte)
- [Datenmodell](#-datenmodell)
- [Entwicklung](#-entwicklung)
- [Lizenz](#-lizenz)

## 🎯 Übersicht

TemuLinks ermöglicht es Benutzern, interessante Produkte von Temu.com als Links zu speichern und zu verwalten. Links können privat gehalten oder mit anderen Benutzern geteilt werden. Die Anwendung bietet:

- 🌐 **Webbasiertes Interface** zur Verwaltung gespeicherter Links
- 🔌 **Browser-Extensions** für Chrome und Edge zum schnellen Speichern von Links
- 🔐 **Benutzerverwaltung** mit Rollen (Admin/User)
- 📊 **Öffentliche Links** zur Entdeckung von Produkten anderer Benutzer
- 🔑 **API-Key-Authentifizierung** für sichere API-Zugriffe

## 📦 Komponenten

### 🌐 WWW (Webanwendung)

**Verzeichnis:** `src/TemuLinks.WWW`

Die Webanwendung ist eine **Blazor WebAssembly**-Anwendung, die das Frontend für TemuLinks bereitstellt.

#### Features:
- ✅ **Benutzerregistrierung und -anmeldung**
- ✅ **Profilverwaltung** (Vorname, Nachname, Passwort ändern)
- ✅ **Link-Verwaltung** mit Grid-Ansicht (eigene Links)
- ✅ **Öffentliche Links-Ansicht** (Links aller Benutzer)
- ✅ **API-Key-Generierung** für Browser-Extensions
- ✅ **Benutzerverwaltung** (nur für Admins)
  - Benutzer aktivieren/deaktivieren
  - Benutzer bearbeiten

#### Hauptseiten:
- **Home** (`Pages/Home.razor`): Startseite mit Übersicht
- **Links** (`Pages/Links.razor`): Verwaltung der eigenen Links
- **PublicLinks** (`Pages/PublicLinks.razor`): Öffentliche Links aller Benutzer
- **Settings** (`Pages/Settings.razor`): API-Key-Verwaltung und Profildaten
- **Users** (`Pages/Users.razor`): Benutzerverwaltung (Admin)
- **Register** (`Pages/Register.razor`): Registrierung neuer Benutzer

#### Technische Details:
- **Framework:** .NET 8 Blazor WebAssembly
- **UI:** Bootstrap 5
- **HTTP-Client:** Eigener `TemuLinksApiClient` für API-Kommunikation
- **Authentifizierung:** `AuthService` mit Session-Management
- **Deployment:** Statische Dateien, kann auf jedem Webserver gehostet werden

---

### 🚀 WebAPI

**Verzeichnis:** `src/TemuLinks.WebAPI`

Die WebAPI ist eine **ASP.NET Core Web API**, die alle Backend-Logik und Datenbankzugriffe bereitstellt.

#### Features:
- ✅ **RESTful API** für alle CRUD-Operationen
- ✅ **API-Key-Authentifizierung** mittels Middleware
- ✅ **Entity Framework Core** mit Code-First-Ansatz
- ✅ **SQLite-Datenbank** (für lokale Entwicklung)
- ✅ **CORS-Support** für Browser-Extensions und WWW
- ✅ **Swagger/OpenAPI** Dokumentation

#### Controller:
- **AuthController** (`Controllers/AuthController.cs`): Login und Authentifizierung
- **TemuLinksController** (`Controllers/TemuLinksController.cs`): CRUD für Links
- **UsersController** (`Controllers/UsersController.cs`): Benutzerverwaltung
- **ApiKeysController** (`Controllers/ApiKeysController.cs`): API-Key-Verwaltung
- **ProfileController** (`Controllers/ProfileController.cs`): Profildaten
- **HealthController** (`Controllers/HealthController.cs`): Health-Check

#### Services:
- **TemuLinkService** (`Services/TemuLinkService.cs`): Geschäftslogik für Links
- **ApiKeyService** (`Services/ApiKeyService.cs`): API-Key-Generierung und -Verwaltung
- **PasswordHasher** (`Services/PasswordHasher.cs`): Sichere Passwort-Hashes mit BCrypt

#### Middleware:
- **ApiKeyAuthenticationMiddleware** (`Middleware/ApiKeyAuthenticationMiddleware.cs`): Validiert API-Keys für geschützte Endpunkte

#### Technische Details:
- **Framework:** ASP.NET Core 8
- **Datenbank:** SQLite (lokal) / SQL Server (Production)
- **ORM:** Entity Framework Core 8
- **Authentifizierung:** API-Key-basiert
- **Passwort-Hashing:** BCrypt
- **Port:** `http://localhost:5002` (Development)

---

### 🗄️ DAL (Data Access Layer)

**Verzeichnis:** `src/TemuLinks.DAL`

Das Data Access Layer ist eine **.NET 8 Class Library**, die das gemeinsame Datenmodell für alle Anwendungen definiert.

#### Features:
- ✅ **Code-First Entity Framework**-Modelle
- ✅ **Gemeinsames Datenmodell** für alle Projekte
- ✅ **Entity Framework Migrations**
- ✅ **DbContext** mit Konfiguration

#### Entities:
- **User** (`Entities/User.cs`): Benutzerdaten, Rollen, Authentifizierung
- **TemuLink** (`Entities/TemuLink.cs`): Gespeicherte Temu-Links
- **ApiKey** (`Entities/ApiKey.cs`): API-Keys für Authentifizierung

#### DbContext:
- **TemuLinksDbContext** (`TemuLinksDbContext.cs`): Haupt-DbContext mit allen Entities

#### Technische Details:
- **Framework:** .NET 8 Class Library
- **ORM:** Entity Framework Core 8
- **Datenbank-Provider:** Microsoft.EntityFrameworkCore.Sqlite
- **Migrations:** Automatische Code-First-Migrationen

---

### 🔌 Browser-Plugins

**Verzeichnis:** `src/TemuLinks.Plugins`

Browser-Extensions für **Chrome** und **Edge**, die das schnelle Speichern von Temu-Links ermöglichen.

#### Features:
- ✅ **One-Click-Button** auf Temu.com-Seiten
- ✅ **Popup-Interface** zum Speichern von Links
- ✅ **API-Konfiguration** (Endpunkt und API-Key)
- ✅ **Link-Counter** zeigt Anzahl gespeicherter Links
- ✅ **Direktlink zur Webanwendung**
- ✅ **Öffentlich/Privat-Option** für Links
- ✅ **Optionale Beschreibung** für jeden Link

#### Komponenten:
- **manifest.json**: Extension-Konfiguration (Chrome/Edge)
- **popup.html/js**: Hauptinterface der Extension
- **options.html/js**: Einstellungsseite für API-Konfiguration
- **content.js/css**: Injiziert Button auf Temu.com-Seiten
- **background.js**: Service Worker für API-Kommunikation

#### Unterschiede Chrome vs. Edge:
- **Chrome**: Manifest V3, `chrome.storage.local` API
- **Edge**: Manifest V3, kompatibel mit Chrome-Extensions

#### Installation:
Siehe [PLUGIN_INSTALLATION.md](PLUGIN_INSTALLATION.md) für detaillierte Installationsanweisungen.

#### Downloads:
- **Chrome**: [TemuLinks-Chrome-v1.0.0.zip](https://github.com/Traxxel/TemuLinks/releases/latest/download/TemuLinks-Chrome-v1.0.0.zip)
- **Edge**: [TemuLinks-Edge-v1.0.0.zip](https://github.com/Traxxel/TemuLinks/releases/latest/download/TemuLinks-Edge-v1.0.0.zip)

## ✨ Features

### Für alle Benutzer:
- 📝 **Registrierung** mit Vorname, Nachname, Benutzername, Passwort
- 🔐 **Login** mit Benutzername und Passwort
- 🔗 **Links speichern** via Browser-Extension oder Webanwendung
- 📝 **Links verwalten** (Erstellen, Bearbeiten, Löschen)
- 🌍 **Links teilen** (öffentlich/privat)
- 👁️ **Öffentliche Links** anderer Benutzer ansehen
- 🔑 **API-Keys generieren** für Browser-Extensions
- 👤 **Profil bearbeiten** (Vorname, Nachname, Passwort)

### Für Admins:
- 👥 **Benutzerverwaltung**
- ✅ **Benutzer aktivieren/deaktivieren**
- ✏️ **Benutzerdaten bearbeiten**

### Sicherheit:
- 🔒 **BCrypt-Passwort-Hashing**
- 🔑 **API-Key-Authentifizierung**
- 🛡️ **Rollen-basierte Zugriffskontrolle**
- 🔐 **Automatische Admin-Rolle** für ersten Benutzer

## 🛠️ Technologie-Stack

### Backend:
- **ASP.NET Core 8** Web API
- **Entity Framework Core 8** (Code-First)
- **SQLite** / SQL Server
- **BCrypt.Net** für Passwort-Hashing

### Frontend:
- **Blazor WebAssembly** (.NET 8)
- **Bootstrap 5**
- **Razor Components**

### Browser-Extensions:
- **Manifest V3** (Chrome/Edge)
- **JavaScript (ES6+)**
- **Chrome Storage API**
- **Chrome Extensions API**

### Entwicklung:
- **.NET 8 SDK**
- **Entity Framework Tools**
- **Visual Studio 2022** / VS Code

## 🏗️ Architektur

```
┌─────────────────────────────────────────────────────┐
│                  Browser-Extensions                  │
│              (Chrome/Edge Plugins)                   │
└──────────────────┬──────────────────────────────────┘
                   │ HTTP (API-Key Auth)
                   │
┌──────────────────▼──────────────────────────────────┐
│              Blazor WebAssembly (WWW)                │
│            (Frontend - Static Files)                 │
└──────────────────┬──────────────────────────────────┘
                   │ HTTP (API-Key Auth)
                   │
┌──────────────────▼──────────────────────────────────┐
│           ASP.NET Core Web API (WebAPI)              │
│        (Backend - RESTful API Server)                │
└──────────────────┬──────────────────────────────────┘
                   │ Entity Framework Core
                   │
┌──────────────────▼──────────────────────────────────┐
│            Data Access Layer (DAL)                   │
│         (Entities, DbContext, Migrations)            │
└──────────────────┬──────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────┐
│                SQLite / SQL Server                   │
│                   (Datenbank)                        │
└─────────────────────────────────────────────────────┘
```

## 🚀 Installation & Setup

### Voraussetzungen:
- **.NET 8 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **SQLite** (bereits in .NET enthalten)
- **Node.js** (optional, für Browser-Extension-Entwicklung)

### 1. Repository klonen:
```bash
git clone https://github.com/Traxxel/TemuLinks.git
cd TemuLinks
```

### 2. Datenbank initialisieren:
```bash
cd src/TemuLinks.WebAPI
dotnet ef database update
```

### 3. WebAPI starten:
```bash
cd src/TemuLinks.WebAPI
dotnet run
```
Die API läuft nun unter `http://localhost:5002`

### 4. Webanwendung starten:
```bash
cd src/TemuLinks.WWW
dotnet run
```
Die Webanwendung läuft nun unter `http://localhost:5000`

### 5. Browser-Extension installieren:
Siehe [PLUGIN_INSTALLATION.md](PLUGIN_INSTALLATION.md) für detaillierte Anweisungen.

## 📖 Verwendung

### 1. Registrierung:
1. Öffne `http://localhost:5000`
2. Klicke auf **"Neu registrieren"**
3. Gib Vorname, Nachname, Benutzername und Passwort ein
4. Der erste Benutzer wird automatisch als Admin aktiviert

### 2. API-Key generieren:
1. Melde dich an
2. Gehe zu **"Einstellungen"**
3. Klicke auf **"Neuen API-Schlüssel generieren"**
4. Kopiere den generierten API-Key

### 3. Browser-Extension konfigurieren:
1. Installiere die Extension (siehe [PLUGIN_INSTALLATION.md](PLUGIN_INSTALLATION.md))
2. Klicke auf das Extension-Icon
3. Klicke auf **"API-Einstellungen konfigurieren"**
4. Gib API-Endpunkt (`http://localhost:5002`) und API-Key ein
5. Klicke auf **"Konfiguration speichern"**

### 4. Links speichern:
1. Besuche eine Seite auf **temu.com**
2. Klicke auf den **"📌 Save to TemuLinks"**-Button
3. Gib optional eine Beschreibung ein
4. Wähle, ob der Link öffentlich sein soll
5. Klicke auf **"Link speichern"**

### 5. Links verwalten:
1. Öffne die Webanwendung
2. Gehe zu **"Meine Links"**
3. Bearbeite oder lösche Links nach Belieben

## 🔗 API-Endpunkte

### Authentifizierung:
- `POST /api/auth/login` - Benutzer-Login
- `POST /api/auth/register` - Benutzer-Registrierung

### Links:
- `GET /api/temulinks` - Alle Links des authentifizierten Benutzers
- `GET /api/temulinks/{id}` - Einzelnen Link abrufen
- `POST /api/temulinks` - Neuen Link erstellen
- `PUT /api/temulinks/{id}` - Link bearbeiten
- `DELETE /api/temulinks/{id}` - Link löschen
- `GET /api/temulinks/public` - Alle öffentlichen Links
- `GET /api/temulinks/count` - Anzahl der Links des Benutzers

### Benutzer:
- `GET /api/users` - Alle Benutzer (Admin)
- `GET /api/users/{id}` - Einzelnen Benutzer abrufen (Admin)
- `PUT /api/users/{id}` - Benutzer bearbeiten (Admin)
- `PUT /api/users/{id}/activate` - Benutzer aktivieren (Admin)
- `PUT /api/users/{id}/deactivate` - Benutzer deaktivieren (Admin)

### Profil:
- `GET /api/profile` - Eigenes Profil abrufen
- `PUT /api/profile` - Eigenes Profil bearbeiten
- `PUT /api/profile/password` - Passwort ändern

### API-Keys:
- `GET /api/apikeys` - Alle API-Keys des Benutzers
- `POST /api/apikeys` - Neuen API-Key generieren
- `DELETE /api/apikeys/{id}` - API-Key löschen

### Health:
- `GET /api/health` - Health-Check

## 📊 Datenmodell

### User:
```csharp
{
  "Id": int,
  "Username": string,
  "PasswordHash": string,
  "FirstName": string,
  "LastName": string,
  "IsActive": bool,
  "IsAdmin": bool,
  "CreatedAt": DateTime
}
```

### TemuLink:
```csharp
{
  "Id": int,
  "Url": string,
  "Description": string,
  "IsPublic": bool,
  "UserId": int,
  "CreatedAt": DateTime
}
```

### ApiKey:
```csharp
{
  "Id": int,
  "KeyValue": string,
  "UserId": int,
  "Name": string,
  "CreatedAt": DateTime,
  "LastUsedAt": DateTime?
}
```

## 🔧 Entwicklung

### Projekt-Struktur:
```
TemuLinks/
├── src/
│   ├── TemuLinks.DAL/          # Data Access Layer
│   ├── TemuLinks.WebAPI/       # ASP.NET Core Web API
│   ├── TemuLinks.WWW/          # Blazor WebAssembly App
│   └── TemuLinks.Plugins/      # Browser-Extensions
│       ├── Chrome/
│       └── Edge/
├── _00_Projektbeschreibung.md  # Projektdokumentation
├── _01_WWW.md                  # WWW-Todos
├── PLUGIN_INSTALLATION.md      # Plugin-Installationsanleitung
├── RELEASE_INSTRUCTIONS.md     # Release-Anleitung
└── README.md                   # Diese Datei
```

### Migrations erstellen:
```bash
cd src/TemuLinks.WebAPI
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Build:
```bash
# Gesamtes Projekt
dotnet build

# Einzelne Komponenten
dotnet build src/TemuLinks.WebAPI
dotnet build src/TemuLinks.WWW
```

### Publish:
```bash
# WebAPI
cd src/TemuLinks.WebAPI
dotnet publish -c Release

# WWW
cd src/TemuLinks.WWW
dotnet publish -c Release
```

## 📝 Lizenz

Dieses Projekt ist für private Nutzung vorgesehen.

---

**Entwickelt mit ❤️ und .NET 8**

