# UsersProductsCartsDemo
Minimal backend demo: Users, Products (DummyJSON), Favorites, Carts.

## Run with Docker

Prereqs
- Docker Desktop
- .NET 9 SDK (see `global.json`)
- (Optional) EF Core CLI: install the tool matching EF version (recommended project-local)
  - Global: `dotnet tool install --global dotnet-ef --version 9.0.2`
  - Project-local (recommended): 
    ```
    dotnet new tool-manifest
    dotnet tool install dotnet-ef --version 9.0.2
    dotnet tool restore
    ```

###  Quick start — Docker Compose (API + SQL Server)

1) Build & start containers: `docker compose up --build`
   
2) Apply EF Core migrations (first run)
- From your machine (solution root) run the __dotnet ef__ command:: `dotnet ef database update 
--project ./AbySalto.Mid.Infrastructure/AbySalto.Mid.Infrastructure.csproj 
--startup-project ./AbySalto.Mid/AbySalto.Mid.WebApi.csproj 
--connection "Server=localhost,1433;Database=AbySaltoMid;User Id=sa;Password=Your_password123;TrustServerCertificate=True"`

3) Open API / Swagger UI
- Swagger UI: http://localhost:8080/index.html
- Swagger JSON: http://localhost:8080/swagger/v1/swagger.json

4) Authenticate in Swagger:
- Use POST /api/auth/register or /api/auth/login
- Click “Authorize” and paste: `Bearer <your_jwt_token>`

Environment variables used (compose):
- ExternalApis__BaseUrl: https://dummyjson.com
- ConnectionStrings__DefaultConnection: SQL Server connection string
- Jwt__Secret / Jwt__Issuer / Jwt__Audience: JWT config
- CacheSettings — TTLs for product caching


