using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VoxelSpace.Api.Infrastructure.Data;
using VoxelSpace.Api.Infrastructure.Repositories;
using VoxelSpace.Api.Interfaces;
using VoxelSpace.Api.Middleware;
using VoxelSpace.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Banco de dados (provider configurável: Oracle | Sqlite) ----------------
var provider = DbProviderSetup.ProviderConfigurado(builder.Configuration);
builder.Services.AddDbContext<VoxelDbContext>(options =>
    DbProviderSetup.Configurar(options, provider, builder.Configuration));

// ---------------- Injeção de dependência (sempre por interface) ----------------
builder.Services.AddScoped<IComponenteRepository, ComponenteRepository>();
builder.Services.AddScoped<IExecucaoRepository, ExecucaoRepository>();
builder.Services.AddScoped<IExecucaoService, ExecucaoService>();
builder.Services.AddScoped<IEstatisticaService, EstatisticaService>();

// ---------------- MVC / JSON ----------------
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        // Enums como texto ("Concluida") em vez de número, para respostas legíveis.
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VoxelSpace API",
        Version = "v1",
        Description = "API de otimização topológica de estruturas espaciais — " +
                      "componentes, execuções, telemetria de convergência e analytics de massa/custo de lançamento."
    });
});

var app = builder.Build();

// ---------------- Pipeline ----------------
// Middleware global de exceções primeiro, para capturar tudo que vier abaixo.
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger habilitado sempre (facilita as evidências de execução).
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoxelSpace API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.MapGet("/healthz", () => Results.Ok(new { ok = true, servico = "VoxelSpace API" }));

// ---------------- Criação do schema + seed (resiliente a falhas de banco) ----------------
await InicializarBancoAsync(app, provider);

app.Run();

static async Task InicializarBancoAsync(WebApplication app, string provider)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var contexto = scope.ServiceProvider.GetRequiredService<VoxelDbContext>();

    try
    {
        if (string.Equals(provider, DbProviderSetup.ProviderSqlite, StringComparison.OrdinalIgnoreCase))
        {
            // SQLite: cria o schema direto a partir do modelo (sem migrations).
            await contexto.Database.EnsureCreatedAsync();
        }
        else
        {
            // Oracle: aplica as migrations versionadas.
            await contexto.Database.MigrateAsync();
        }

        await DataSeeder.SemearAsync(contexto, logger);
    }
    catch (Exception ex)
    {
        // Não derruba a API: registra e segue. Útil quando o Oracle ainda não subiu.
        logger.LogError(ex,
            "Falha ao inicializar o banco ({Provider}). A API segue no ar; verifique a conexão.", provider);
    }
}
