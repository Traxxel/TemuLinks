# Technische Vorgehensweise - TemuLinks Projekt

## Phase 1: Projektstruktur und Grundlagen

### 1.1 Solution-Struktur erstellen

```
TemuLinks/
├── src/
│   ├── TemuLinks.DAL/           # Data Access Layer
│   ├── TemuLinks.WebAPI/        # Web API
│   ├── TemuLinks.Web/           # Blazor Web App
│   └── TemuLinks.Plugins/
│       └── Chrome/              # Chrome Extension
├── docs/                        # Dokumentation
└── tests/                       # Unit Tests
```

### 1.2 DAL-Projekt (TemuLinks.DAL)

- **Ziel**: Entity Framework Core Modelle und DbContext
- **Technologien**: .NET 8, Entity Framework Core, MySQL
- **Entitäten**:
  - `User` (Id, Username, PasswordHash, FirstName, LastName, IsActive, Role, CreatedAt)
  - `TemuLink` (Id, UserId, Url, Description, IsPublic, CreatedAt)

### 1.3 WebAPI-Projekt (TemuLinks.WebAPI)

- **Ziel**: REST API mit Authentifizierung
- **Technologien**: .NET 8, ASP.NET Core Web API, Entity Framework Core
- **Features**:
  - JWT Token Authentifizierung
  - CRUD-Operationen für TemuLinks
  - Swagger/OpenAPI Dokumentation

### 1.4 Blazor Web App (TemuLinks.Web)

- **Ziel**: Benutzeroberfläche für Verwaltung
- **Technologien**: .NET 8, Blazor Server, ASP.NET Core Identity
- **Features**:
  - Benutzername/Passwort Anmeldung
  - Benutzerverwaltung (Admin)
  - Benutzerregistrierung
  - TemuLink Verwaltung (Grid)
  - Öffentliche Links anzeigen

### 1.5 Chrome Extension (TemuLinks.Plugins.Chrome)

- **Ziel**: Browser-Plugin für Temu.com
- **Technologien**: Manifest V3, JavaScript, HTML, CSS
- **Features**:
  - Konfiguration (API-Endpoint, Anmeldedaten)
  - Temu.com Erkennung
  - Link speichern Dialog
  - Link zur Webanwendung
  - Sichere Speicherung der Anmeldedaten

## Phase 2: Datenbank und DAL

### 2.1 Entity Framework Setup

1. **DAL-Projekt erstellen**

   ```bash
   dotnet new classlib -n TemuLinks.DAL
   cd TemuLinks.DAL
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. **Entitäten definieren**

   - User-Klasse mit Username/PasswordHash
   - TemuLink-Klasse für Links
   - Identity-Integration für Authentifizierung

3. **DbContext erstellen**
   - TemuLinksDbContext
   - OnModelCreating Konfiguration
   - Seed-Daten für ersten Admin

### 2.2 Datenbankverbindung

1. **Connection String in appsettings.json**

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Port=3306;Database=temulinks;Uid=root;Pwd=Geheim;"
     }
   }
   ```

2. **Migration erstellen**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

## Phase 3: WebAPI Entwicklung

### 3.1 API-Projekt Setup

1. **WebAPI-Projekt erstellen**

   ```bash
   dotnet new webapi -n TemuLinks.WebAPI
   cd TemuLinks.WebAPI
   dotnet add reference ../TemuLinks.DAL
   ```

2. **Dependencies hinzufügen**
   ```bash
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Swashbuckle.AspNetCore
   ```

### 3.2 API-Controller entwickeln

1. **TemuLinksController**

   - GET /api/links (alle Links des Benutzers)
   - POST /api/links (neuen Link erstellen)
   - PUT /api/links/{id} (Link bearbeiten)
   - DELETE /api/links/{id} (Link löschen)
   - GET /api/links/public (öffentliche Links)
   - GET /api/links/count (Anzahl Links)

2. **Authentication Middleware**
   - JWT Token Validierung
   - User-Context aus JWT Token

### 3.3 DTOs und Services

1. **DTOs erstellen**

   - TemuLinkDto
   - CreateTemuLinkDto
   - UpdateTemuLinkDto

2. **Services implementieren**
   - ITemuLinkService
   - TemuLinkService
   - Dependency Injection konfigurieren

## Phase 4: Blazor Web App

### 4.1 Blazor-Projekt Setup

1. **Blazor Server App erstellen**

   ```bash
   dotnet new blazorserver -n TemuLinks.Web
   cd TemuLinks.Web
   dotnet add reference ../TemuLinks.DAL
   ```

2. **ASP.NET Core Identity konfigurieren**
   - Identity Services konfigurieren
   - appsettings.json Konfiguration
   - Password-Hashing implementieren

### 4.2 Blazor-Komponenten

1. **Layout und Navigation**

   - MainLayout.razor
   - NavMenu.razor
   - Responsive Design

2. **Benutzerverwaltung (Admin)**

   - UserManagement.razor
   - UserEdit.razor
   - Benutzerregistrierung

3. **TemuLink Verwaltung**
   - TemuLinkList.razor (Grid)
   - TemuLinkEdit.razor
   - PublicLinks.razor

### 4.3 Services und HttpClient

1. **API-Client Service**
   - HttpClient für API-Aufrufe
   - JWT Token Authentication
   - Error Handling

## Phase 5: Chrome Extension

### 5.1 Extension-Struktur

```
Chrome/
├── manifest.json
├── popup.html
├── popup.js
├── content.js
├── background.js
├── options.html
├── options.js
└── styles.css
```

### 5.2 Manifest V3 Setup

1. **manifest.json konfigurieren**

   - Permissions für temu.com
   - Content Scripts
   - Background Service Worker

2. **Content Script (content.js)**
   - Temu.com Erkennung
   - Button Injection
   - Event Handling

### 5.3 Popup Interface

1. **popup.html/popup.js**

   - Konfiguration (API-Endpoint, Anmeldedaten)
   - Link speichern Dialog
   - Status-Anzeige

2. **Options Page**
   - Erweiterte Konfiguration
   - Anmeldedaten Eingabe
   - Test-Verbindung
   - Chrome Storage API Integration

## Phase 6: Integration und Testing

### 6.1 API-Integration

1. **Chrome Extension → WebAPI**

   - HTTP-Requests mit JWT Token
   - Error Handling
   - Chrome Storage API für Anmeldedaten

2. **Blazor Web → WebAPI**
   - HttpClient Konfiguration
   - JWT Token Authentication Flow

### 6.2 Testing

1. **Unit Tests**

   - DAL Tests
   - API Controller Tests
   - Service Tests

2. **Integration Tests**
   - End-to-End API Tests
   - Browser Extension Tests

## Phase 7: Deployment und Dokumentation

### 7.1 Lokale Entwicklung

1. **Datenbank Setup**

   - MySQL Server Installation
   - Datenbank erstellen
   - Migration ausführen

2. **Anwendung starten**
   - WebAPI starten
   - Blazor App starten
   - Chrome Extension laden

### 7.2 Dokumentation

1. **Setup-Anleitung**

   - Entwicklungsumgebung
   - Datenbank-Konfiguration
   - Extension Installation

2. **API-Dokumentation**
   - Swagger/OpenAPI
   - Endpoint-Beschreibungen
   - Authentication

## Technologie-Stack

### Backend

- **.NET 8** - Framework
- **ASP.NET Core Web API** - REST API
- **Entity Framework Core** - ORM
- **MySQL** - Datenbank
- **Blazor Server** - Web UI

### Frontend

- **Blazor Server** - Webanwendung
- **ASP.NET Core Identity** - Login
- **Bootstrap** - UI Framework
- **JavaScript** - Chrome Extension
- **Chrome Storage API** - Sichere Datenspeicherung

### Tools

- **Visual Studio 2022** - IDE
- **MySQL Workbench** - Datenbank
- **Chrome Developer Tools** - Extension Debugging
- **Swagger** - API Dokumentation

## Nächste Schritte

1. **Projektstruktur erstellen** (Phase 1)
2. **DAL und Datenbank** (Phase 2)
3. **WebAPI entwickeln** (Phase 3)
4. **Blazor App** (Phase 4)
5. **Chrome Extension** (Phase 5)
6. **Integration testen** (Phase 6)
7. **Dokumentation** (Phase 7)
