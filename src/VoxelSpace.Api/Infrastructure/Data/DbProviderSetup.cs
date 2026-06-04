using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace VoxelSpace.Api.Infrastructure.Data;

/// <summary>
/// Centraliza a escolha do provider EF Core a partir da configuração. Suporta
/// Oracle (alvo de produção, XE em Docker) e SQLite (fallback local para
/// desenvolvimento/evidências sem Docker).
/// </summary>
public static class DbProviderSetup
{
    public const string ProviderOracle = "Oracle";
    public const string ProviderSqlite = "Sqlite";

    public static string ProviderConfigurado(IConfiguration configuracao)
        => configuracao["DatabaseProvider"] ?? ProviderOracle;

    public static void Configurar(
        DbContextOptionsBuilder options, string provider, IConfiguration configuracao)
    {
        if (string.Equals(provider, ProviderSqlite, StringComparison.OrdinalIgnoreCase))
        {
            var conexao = configuracao.GetConnectionString("Sqlite") ?? "Data Source=voxelspace.db";
            options.UseSqlite(conexao);
            return;
        }

        // Oracle (padrão).
        var oracle = configuracao.GetConnectionString("Oracle")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:Oracle não configurada. Defina-a ou use DatabaseProvider=Sqlite.");

        // O provider 9.23 usa, por padrão, o dialeto do Oracle 23ai (tipo BOOLEAN e
        // literais TRUE/FALSE). O Oracle XE 21c não os suporta (ORA-00902/ORA-00904),
        // então fixamos a compatibilidade em 21c: booleanos viram NUMBER(1) e 1/0.
        options.UseOracle(oracle, o => o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21));
    }
}
