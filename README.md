# TouristSpots

Solução em camadas (.NET 8 + ASP.NET Core Web API + EF Core + SQL Server).

## Estrutura
```
TouristSpots.sln
src/
  TouristSpots.Domain/
  TouristSpots.Application/
  TouristSpots.Infrastructure/
  TouristSpots.Api/
```

## Pré-requisitos
- Visual Studio 2022 (ou VS Code) com .NET 8 SDK
- SQL Server local

## Configuração
1. Abra a solução `TouristSpots.sln` no Visual Studio 2022.
2. Ajuste a Connection String em `src/TouristSpots.Api/appsettings.json` se necessário.
3. Restaure os pacotes e compile.
4. Crie e aplique as migrations (Package Manager Console):
   ```powershell
   Add-Migration InitialCreate -Project TouristSpots.Infrastructure -StartupProject TouristSpots.Api
   Update-Database -Project TouristSpots.Infrastructure -StartupProject TouristSpots.Api
   ```
   Ou via CLI:
   ```bash
   dotnet ef migrations add InitialCreate      --project ./src/TouristSpots.Infrastructure      --startup-project ./src/TouristSpots.Api
   dotnet ef database update      --project ./src/TouristSpots.Infrastructure      --startup-project ./src/TouristSpots.Api
   ```
5. Execute a API (`TouristSpots.Api`). O Swagger abrirá em `/swagger`.

## Endpoints
- `POST /api/tourist-spots`
- `GET /api/tourist-spots?term=&page=&pageSize=`
- `GET /api/tourist-spots/{id}`

## Observações
- Este pacote não inclui frontend. A API está pronta para ser consumida por um SPA (React) ou páginas server-rendered.

