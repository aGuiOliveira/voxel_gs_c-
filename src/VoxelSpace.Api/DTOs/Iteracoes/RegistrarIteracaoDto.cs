using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.DTOs.Iteracoes;

/// <summary>Amostra de telemetria de uma iteração do solver.</summary>
public sealed class RegistrarIteracaoDto
{
    public int NumeroIteracao { get; set; }
    public double Compliance { get; set; }
    public double ComplianceDelta { get; set; }
    public double VolumeFracao { get; set; }
    public double Change { get; set; }
    public double TempoIteracaoS { get; set; }

    public IteracaoOtimizacao ParaDominio() => new()
    {
        NumeroIteracao = NumeroIteracao,
        Compliance = Compliance,
        ComplianceDelta = ComplianceDelta,
        VolumeFracao = VolumeFracao,
        Change = Change,
        TempoIteracaoS = TempoIteracaoS
    };
}
