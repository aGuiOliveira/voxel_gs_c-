using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.DTOs.Estatisticas;

/// <summary>Redução e economia agregadas por material aeroespacial.</summary>
public sealed record EstatisticaPorMaterialDto
{
    public MaterialAeroespacial Material { get; init; }
    public string MaterialNome { get; init; } = string.Empty;
    public int QtdExecucoes { get; init; }
    public double ReducaoMediaPct { get; init; }
    public double ReducaoMaximaPct { get; init; }
    public decimal MassaEconomizadaKg { get; init; }
    public decimal CustoEconomizadoUsd { get; init; }
}
