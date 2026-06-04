using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VoxelSpace.Api.Infrastructure.Data;

/// <summary>
/// Fábrica usada pelas ferramentas de design-time do EF Core (dotnet ef) para
/// criar o contexto fora do pipeline web — assim 'migrations add' não dispara o
/// startup/seed da aplicação nem exige um banco no ar.
/// </summary>
public sealed class VoxelDbContextFactory : IDesignTimeDbContextFactory<VoxelDbContext>
{
    public VoxelDbContext CreateDbContext(string[] args)
    {
        var configuracao = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var provider = configuracao["DatabaseProvider"] ?? "Oracle";
        var optionsBuilder = new DbContextOptionsBuilder<VoxelDbContext>();
        DbProviderSetup.Configurar(optionsBuilder, provider, configuracao);

        return new VoxelDbContext(optionsBuilder.Options);
    }
}
