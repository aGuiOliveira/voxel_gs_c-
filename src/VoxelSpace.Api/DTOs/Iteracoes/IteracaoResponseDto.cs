using VoxelSpace.Api.Domain.Common;
using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.DTOs.Iteracoes;

/// <summary>Representação de saída de uma iteração (ponto da curva de convergência).</summary>
public sealed record IteracaoResponseDto
{
    public int Id { get; init; }
    public int ExecucaoId { get; init; }
    public int NumeroIteracao { get; init; }
    public double Compliance { get; init; }
    public double ComplianceDelta { get; init; }
    public double VolumeFracao { get; init; }
    public double Change { get; init; }
    public double TempoIteracaoS { get; init; }
    public DateTime RegistradoEmUtc { get; init; }
    public DateTime RegistradoEmLocal { get; init; }

    public static IteracaoResponseDto De(IteracaoOtimizacao i) => new()
    {
        Id = i.Id,
        ExecucaoId = i.ExecucaoId,
        NumeroIteracao = i.NumeroIteracao,
        Compliance = i.Compliance,
        ComplianceDelta = i.ComplianceDelta,
        VolumeFracao = i.VolumeFracao,
        Change = i.Change,
        TempoIteracaoS = i.TempoIteracaoS,
        RegistradoEmUtc = i.RegistradoEm,
        RegistradoEmLocal = FusoHorario.ParaBrasilia(i.RegistradoEm)
    };
}
