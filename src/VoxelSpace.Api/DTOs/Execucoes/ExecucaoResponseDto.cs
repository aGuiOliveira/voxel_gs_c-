using VoxelSpace.Api.Domain.Common;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.DTOs.Execucoes;

/// <summary>Representação de saída completa de uma execução de otimização.</summary>
public sealed record ExecucaoResponseDto
{
    public int Id { get; init; }
    public int ComponenteId { get; init; }
    public string? ComponenteNome { get; init; }
    public StatusExecucao Status { get; init; }
    public string StatusNome { get; init; } = string.Empty;

    public DateTime DataCriacaoUtc { get; init; }
    public DateTime? DataInicioUtc { get; init; }
    public DateTime? DataFimUtc { get; init; }
    public DateTime DataCriacaoLocal { get; init; }
    public double? DuracaoSegundos { get; init; }

    public ParametrosDto Parametros { get; init; } = new();

    // Resultados (preenchidos quando concluída).
    public double? VolumeDesignSpaceMm3 { get; init; }
    public double? VolumeFinalMm3 { get; init; }
    public double? ReducaoPct { get; init; }
    public double? EffectiveVolfrac { get; init; }
    public bool? Watertight { get; init; }
    public int? GridX { get; init; }
    public int? GridY { get; init; }
    public int? GridZ { get; init; }
    public int? NComponentsDiscarded { get; init; }
    public double? SolverElapsedS { get; init; }
    public decimal? MassaFinalKg { get; init; }
    public decimal? MassaEconomizadaKg { get; init; }
    public decimal? CustoEconomizadoUsd { get; init; }

    public string? MensagemErro { get; init; }
    public int QtdIteracoes { get; init; }
    public int QtdRegioes { get; init; }

    public static ExecucaoResponseDto De(ExecucaoOtimizacao e)
    {
        var massaEconomizada = e.MassaEconomizadaKg();
        return new ExecucaoResponseDto
        {
            Id = e.Id,
            ComponenteId = e.ComponenteId,
            ComponenteNome = e.Componente?.Nome,
            Status = e.Status,
            StatusNome = e.Status.ToString(),
            DataCriacaoUtc = e.DataCriacao,
            DataInicioUtc = e.DataInicio,
            DataFimUtc = e.DataFim,
            DataCriacaoLocal = FusoHorario.ParaBrasilia(e.DataCriacao),
            DuracaoSegundos = e.DuracaoSegundos,
            Parametros = ParametrosDto.De(e.Parametros),
            VolumeDesignSpaceMm3 = e.VolumeDesignSpaceMm3,
            VolumeFinalMm3 = e.VolumeFinalMm3,
            ReducaoPct = e.ReducaoPct,
            EffectiveVolfrac = e.EffectiveVolfrac,
            Watertight = e.Watertight,
            GridX = e.GridX,
            GridY = e.GridY,
            GridZ = e.GridZ,
            NComponentsDiscarded = e.NComponentsDiscarded,
            SolverElapsedS = e.SolverElapsedS,
            MassaFinalKg = e.MassaFinalKg,
            MassaEconomizadaKg = massaEconomizada,
            CustoEconomizadoUsd = massaEconomizada is null
                ? null
                : CalculadoraLancamento.CustoEconomizadoUsd(massaEconomizada.Value),
            MensagemErro = e.MensagemErro,
            QtdIteracoes = e.Iteracoes?.Count ?? 0,
            QtdRegioes = e.Regioes?.Count ?? 0
        };
    }
}
