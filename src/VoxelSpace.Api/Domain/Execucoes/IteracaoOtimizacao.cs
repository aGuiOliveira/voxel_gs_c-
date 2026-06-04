namespace VoxelSpace.Api.Domain.Execucoes;

/// <summary>
/// Telemetria de uma iteração do solver — a série temporal da convergência.
/// Espelha a linha de log "Iter X: Obj=..., Vol=..., change=..." que o engine
/// emite a cada iteração. Várias destas formam a curva de convergência exposta
/// pela API.
/// </summary>
public sealed class IteracaoOtimizacao
{
    public int Id { get; private set; }

    /// <summary>FK para a execução dona desta iteração.</summary>
    public int ExecucaoId { get; set; }

    public ExecucaoOtimizacao? Execucao { get; set; }

    /// <summary>Número sequencial da iteração no solver.</summary>
    public int NumeroIteracao { get; set; }

    /// <summary>Compliance (energia de deformação) — objetivo a minimizar.</summary>
    public double Compliance { get; set; }

    /// <summary>Variação da compliance em relação à iteração anterior.</summary>
    public double ComplianceDelta { get; set; }

    /// <summary>Fração de volume corrente (0..1).</summary>
    public double VolumeFracao { get; set; }

    /// <summary>Maior mudança de densidade entre iterações — critério de parada.</summary>
    public double Change { get; set; }

    /// <summary>Tempo gasto nesta iteração (s).</summary>
    public double TempoIteracaoS { get; set; }

    /// <summary>Instante (UTC) em que a telemetria foi registrada.</summary>
    public DateTime RegistradoEm { get; set; } = DateTime.UtcNow;
}
