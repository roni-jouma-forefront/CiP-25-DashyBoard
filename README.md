Spår B2B - Hotellversion Pilow
Hotellkedjan Pilows riktar sig till trötta piloter världen över och ska starta upp ett nytt koncept precis intill en flygplats. Det är helt upp till er vilken flygplats hotellet ska etablera sig på. Varje rum ska bestyckas med en smart spegel, ni kan anta att det är färre än 100 rum. Det är er uppgift att utveckla systemet för dessa speglar.

I hotellversionen krävs ingen personlig inloggning och vädret bestäms av hotellets position. Däremot kräver personalen en central kontrollstation som kan hantera gästernas speglar/dashboard. Från kontrollstationen ska personal kunna konfigurera rummens speglar efter gästens information. Som till exempel namn och pilotens nästa resmål.

Hotellspegeln innehåller:

Klocka
Lokalväder enl. hotellets ICAO kod
Destination
Väder på flygplatsen
Gate (om finns)
Avgångstid (om finns)
Flygplatsinformation
Arrivals
Departures + Boarding gates
Utöver innehåll ska hotellpersonalen kunna administrera sina enheter via en konsol eller applikation där de ska kunna:

Anpassa gästinformationen till varje rum/enhet
Namn
Nästa destination eller flygning
Skicka ut meddelanden och information som visas på gästernas enheter

DashyBoard features	Hotellversion (B2B)

Klocka	✅
Lokaltrafik (t.ex. SL)	✅
Adminverktyg för att anpassa	✅
ICAO anpassat väder	✅
Flygplatsens boarding gates	✅
Flygplatsens arrivals	✅
Meddelandetjänst main -> sub	✅

# CiP-25-DashyBoard

System Architecture

![fb4f4bcb-4161-47dc-a121-5431d78a2e55](https://github.com/user-attachments/assets/41f7f92b-07c9-4ff7-a6e9-486db438a3c4)

Entity Relationship Diagram

<img width="4134" height="3900" alt="Zebra Inheritance Framework-2026-02-05-084847" src="https://github.com/user-attachments/assets/c8135ef5-81e3-497b-b04b-90bfbb9be7e9" />

1. Naming ConventionsWe will follow the industry standards for C#/.NET and TypeScript/React.

<img width="758" height="306" alt="image" src="https://github.com/user-attachments/assets/18bfe718-9f0c-4e64-9515-990717997161" />

2. Git Workflow & Pull Requests (PRs)

   Skapa branch från Dev -> PR och Merge into Dev -> PR av Dev into Main

<img width="777" height="333" alt="image" src="https://github.com/user-attachments/assets/907a44da-d252-46fc-9f8a-93a5efd1b4c4" />

## 🐳 Docker Setup

### Starting the Application

Run all services (frontend and backend):
```bash
docker-compose up --build
```

### Accessing the Application

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000

### Services

1. **dashyboard-frontend** - React + Vite application served by Nginx
2. **dashyboard-api** - .NET 8 Web API with SQLite database

### Database

The application uses SQLite for data storage. The database file is stored in `backend/data/` directory which is mounted as a volume in Docker, ensuring data persists between container restarts.

#### Running Migrations

To create or update the database schema:

```bash
cd backend/DashyBoard.API
dotnet ef migrations add InitialCreate --project ../DashyBoard.Infrastructure
dotnet ef database update
```

The database will be automatically created in the `backend/data/` directory.

### Development

For local development without Docker:

**Backend:**
```bash
cd backend/DashyBoard.API
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

The frontend development server includes a proxy to the backend API at `/api/*`.



