namespace VoxelSpace.Api.DTOs.Estatisticas;

/// <summary>Agregados globais sobre todas as execuções concluídas.</summary>
public sealed record ResumoEstatisticasDto
{
    public int TotalExecucoes { get; init; }
    public int TotalConcluidas { get; init; }
    public double TaxaWatertightPct { get; init; }
    public double ReducaoMediaPct { get; init; }
    public double ReducaoMaximaPct { get; init; }
    public double TempoMedioSolverS { get; init; }
    public decimal MassaEconomizadaTotalKg { get; init; }
    public decimal CustoEconomizadoTotalUsd { get; init; }
}
