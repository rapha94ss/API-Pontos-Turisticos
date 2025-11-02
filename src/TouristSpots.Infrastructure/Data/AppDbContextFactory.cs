// src/TouristSpots.Infrastructure/Data/AppDbContextFactory.cs
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TouristSpots.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 1) Tenta pegar a connection de variável de ambiente (mais confiável em build/CI)
        var envConn = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULTCONNECTION");
        if (!string.IsNullOrWhiteSpace(envConn))
        {
            return CreateWith(connString: envConn);
        }

        // 2) Tenta ler o appsettings.json do projeto API (Startup Project)
        // Localiza a pasta da solução/projeto a partir do diretório atual do design-time
        var basePath = Directory.GetCurrentDirectory();

        // Caminhos possíveis do appsettings.json da API (ajuste se mudar estrutura)
        // Ex.: quando o design-time roda a partir de ./src/TouristSpots.Infrastructure
        var candidates = new[]
        {
            Path.Combine(basePath, "..", "TouristSpots.Api", "appsettings.json"),
            Path.Combine(basePath, "..", "..", "TouristSpots.Api", "appsettings.json"),
            Path.Combine(basePath, "appsettings.json") // fallback se estiver rodando dentro da Api por acaso
        };

        foreach (var path in candidates)
        {
            if (File.Exists(path))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(path, optional: false, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build();

                var cs = config.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(cs))
                    return CreateWith(cs);
            }
        }

        // 3) Fallback final (edite para sua instância)
        var fallback = "Server=localhost\\SQLEXPRESS;Database=TouristSpotsDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";
        return CreateWith(fallback);
    }

    private static AppDbContext CreateWith(string connString)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connString)
            .Options;

        return new AppDbContext(opts);
    }
}