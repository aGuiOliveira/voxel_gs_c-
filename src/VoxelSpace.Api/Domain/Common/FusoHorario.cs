namespace VoxelSpace.Api.Domain.Common;

/// <summary>
/// Conversões de fuso horário. Os timestamps são persistidos em UTC; aqui os
/// convertemos para o horário de Brasília para exibição amigável nas respostas.
/// </summary>
public static class FusoHorario
{
    // Resolve o id correto em Windows ("E. South America Standard Time") ou
    // Linux/IANA ("America/Sao_Paulo"). Cai para UTC se o SO não conhecer o fuso.
    private static readonly TimeZoneInfo Brasilia = ResolverBrasilia();

    /// <summary>Converte um instante UTC para o horário de Brasília.</summary>
    public static DateTime ParaBrasilia(DateTime utc)
    {
        var emUtc = utc.Kind == DateTimeKind.Utc ? utc : DateTime.SpecifyKind(utc, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(emUtc, Brasilia);
    }

    /// <summary>Versão nullable: retorna null se a entrada for null.</summary>
    public static DateTime? ParaBrasilia(DateTime? utc)
        => utc.HasValue ? ParaBrasilia(utc.Value) : null;

    private static TimeZoneInfo ResolverBrasilia()
    {
        foreach (var id in new[] { "America/Sao_Paulo", "E. South America Standard Time" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
                // tenta o próximo id
            }
            catch (InvalidTimeZoneException)
            {
                // tenta o próximo id
            }
        }

        return TimeZoneInfo.Utc;
    }
}
