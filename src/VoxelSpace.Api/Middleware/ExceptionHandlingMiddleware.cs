using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VoxelSpace.Api.Exceptions;

namespace VoxelSpace.Api.Middleware;

/// <summary>
/// Captura qualquer exceção que escape dos controllers e a converte em uma
/// resposta <see cref="ProblemDetails"/> com o status HTTP apropriado. Garante
/// que a API permaneça no ar e responda de forma previsível diante de entradas
/// inválidas ou falhas — em vez de devolver um stack trace ou derrubar o processo.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RecursoNaoEncontradoException ex)
        {
            await EscreverProblemaAsync(context, StatusCodes.Status404NotFound, "Recurso não encontrado", ex.Message);
        }
        catch (ParametroInvalidoException ex)
        {
            await EscreverProblemaAsync(context, StatusCodes.Status400BadRequest, "Parâmetro inválido", ex.Message);
        }
        catch (RegraNegocioException ex)
        {
            await EscreverProblemaAsync(context, StatusCodes.Status409Conflict, "Conflito de regra de negócio", ex.Message);
        }
        catch (FormatException ex)
        {
            // ex.: conversão de data/número mal formado vindo do cliente.
            _logger.LogWarning(ex, "Entrada mal formada.");
            await EscreverProblemaAsync(context, StatusCodes.Status400BadRequest, "Formato inválido", ex.Message);
        }
        catch (ArgumentException ex)
        {
            // cobre ArgumentNullException e ArgumentOutOfRangeException.
            _logger.LogWarning(ex, "Argumento inválido.");
            await EscreverProblemaAsync(context, StatusCodes.Status400BadRequest, "Argumento inválido", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado ao processar {Metodo} {Caminho}.",
                context.Request.Method, context.Request.Path);
            await EscreverProblemaAsync(context, StatusCodes.Status500InternalServerError,
                "Erro interno", "Ocorreu um erro inesperado. A equipe foi notificada nos logs.");
        }
    }

    private static async Task EscreverProblemaAsync(HttpContext context, int status, string titulo, string detalhe)
    {
        // Se a resposta já começou a ser enviada, não há o que fazer.
        if (context.Response.HasStarted)
        {
            return;
        }

        var problema = new ProblemDetails
        {
            Status = status,
            Title = titulo,
            Detail = detalhe,
            Instance = context.Request.Path
        };

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problema));
    }
}
