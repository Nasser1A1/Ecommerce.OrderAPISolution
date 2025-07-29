# 📦 Order API Microservice

Handles **creation of orders** and integrates with **Product** and **Client** microservices.

---

## ⚠️ Required Dependency

This service **requires** the [Shared Library](https://github.com/Nasser1A1/SharedLibrarySolution) which provides:
- Exception handling
- Logging (Serilog)
- Middleware & DI helpers
- Common response types

Make sure to **clone and reference** it in your project.

---

## 🚀 Setup

1. Clone this repo  
   `git clone https://github.com/your-username/OrderApi.git`

2. Clone & build Shared Library  
   `git clone https://github.com/Nasser1A1/SharedLibrarySolution.git`

3. Add project reference to the Shared Library.

4. Update `appsettings.json`:
```
Run the project 
dotnet run

🔗 Endpoints
POST /api/orders — Create order

GET /api/orders/{id} — Get by ID

GET /api/orders — Get all orders

🧪 Swagger
Test via: https://localhost:5001/swagger

🛠️ Stack
ASP.NET Core

EF Core

JWT Auth (via Gateway)

HttpClient for microservice calls
```
