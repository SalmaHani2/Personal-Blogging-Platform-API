# Personal Blogging Platform API

This is a beginner-friendly ASP.NET Core Web API for a personal blogging platform. It demonstrates common backend patterns: Controllers, Services, Repositories, EF Core, JWT authentication, DTOs, validation, pagination, filtering, and Swagger.

Technologies
- .NET 10
- C#
- ASP.NET Core Web API (Controllers)
- Entity Framework Core (SQL Server)
- JWT Authentication
- Swagger / OpenAPI

Getting started
1. Configure the connection string in appsettings.json (Default uses LocalDB).
2. Update Jwt:Secret in appsettings.json for production.
3. From the Package Manager Console / terminal run:

   dotnet ef migrations add InitialCreate
   dotnet ef database update

4. Run the project (F5) and open Swagger at https://localhost:{port}/swagger

Key endpoints
- POST /api/auth/register
- POST /api/auth/login
- GET /api/articles
- GET /api/articles/{id}
- POST /api/articles (protected)
- PUT /api/articles/{id} (protected)
- DELETE /api/articles/{id} (protected)
- GET /api/tags

Architecture overview
https://roadmap.sh/projects/blogging-platform-api

Client -> Controller -> Service -> Repository/DbContext -> EF Core -> SQL Server

Notes for students
- Passwords are hashed using PBKDF2 and never stored in plain text.
- DTOs separate API models from persistence models.
- ErrorHandlerMiddleware provides consistent error responses.
- Seed data is automatically created on first run for development.
