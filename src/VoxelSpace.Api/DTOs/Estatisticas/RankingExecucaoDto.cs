using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.DTOs.Estatisticas;

/// <summary>Linha do ranking de execuções por redução de massa.</summary>
public sealed record RankingExecucaoDto
{
    public int Posicao { get; init; }
    public int ExecucaoId { get; init; }
    public string ComponenteNome { get; init; } = string.Empty;
    public MaterialAeroespacial Material { get; init; }
    public double ReducaoPct { get; init; }
    public decimal MassaEconomizadaKg { get; init; }
    public decimal CustoEconomizadoUsd { get; init; }
}
