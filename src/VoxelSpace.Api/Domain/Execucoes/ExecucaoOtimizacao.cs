using VoxelSpace.Api.Domain.Common;
using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Regioes;
using VoxelSpace.Api.Exceptions;

namespace VoxelSpace.Api.Domain.Execucoes;

/// <summary>
/// Uma execução de otimização topológica de um componente: agrega os parâmetros
/// do solver, o ciclo de vida (status + timestamps), a telemetria iteração-a-
/// iteração, as condições de contorno e as métricas finais.
///
/// O estado só muda pelos métodos de transição (Iniciar/Concluir/RegistrarFalha/
/// Cancelar) — os setters das propriedades de resultado são privados, garantindo
/// que a peça não termine em um estado inconsistente.
/// </summary>
public sealed class ExecucaoOtimizacao
{
    public int Id { get; private set; }

    // ---------------- componente alvo ----------------
    public int ComponenteId { get; set; }
    public ComponenteEspacial? Componente { get; set; }

    // ---------------- parâmetros ----------------
    public ParametrosOtimizacao Parametros { get; set; } = new();

    // ---------------- ciclo de vida ----------------
    public StatusExecucao Status { get; private set; } = StatusExecucao.Enfileirada;
    public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
    public DateTime? DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public string? MensagemErro { get; private set; }

    // ---------------- resultados ----------------
    public double? VolumeDesignSpaceMm3 { get; private set; }
    public double? VolumeFinalMm3 { get; private set; }
    public double? ReducaoPct { get; private set; }
    public double? EffectiveVolfrac { get; private set; }
    public bool? Watertight { get; private set; }
    public int? GridX { get; private set; }
    public int? GridY { get; private set; }
    public int? GridZ { get; private set; }
    public int? NComponentsDiscarded { get; private set; }
    public double? SolverElapsedS { get; private set; }
    public decimal? MassaFinalKg { get; private set; }

    // ---------------- coleções ----------------
    public ICollection<IteracaoOtimizacao> Iteracoes { get; private set; } = new List<IteracaoOtimizacao>();
    public ICollection<RegiaoFronteira> Regioes { get; private set; } = new List<RegiaoFronteira>();

    /// <summary>
    /// Duração efetiva entre início e fim. Null enquanto não terminou.
    /// Demonstra manipulação de DateTime (diferença de instantes).
    /// </summary>
    public double? DuracaoSegundos
    {
        get
        {
            if (DataInicio is null || DataFim is null)
            {
                return null;
            }

            return (DataFim.Value - DataInicio.Value).TotalSeconds;
        }
    }

    /// <summary>true quando o status é terminal (não admite mais transições).</summary>
    public bool EstaFinalizada =>
        Status is StatusExecucao.Concluida or StatusExecucao.Erro or StatusExecucao.Cancelada;

    /// <summary>Move de Enfileirada para Executando.</summary>
    public void Iniciar()
    {
        if (Status != StatusExecucao.Enfileirada)
        {
            throw new RegraNegocioException(
                $"Só é possível iniciar uma execução enfileirada (status atual: {Status}).");
        }

        Status = StatusExecucao.Executando;
        DataInicio = DateTime.UtcNow;
    }

    /// <summary>
    /// Registra uma amostra de telemetria. Só é aceita enquanto a execução roda.
    /// </summary>
    public void RegistrarIteracao(IteracaoOtimizacao iteracao)
    {
        ArgumentNullException.ThrowIfNull(iteracao);

        if (Status != StatusExecucao.Executando)
        {
            throw new RegraNegocioException(
                $"Telemetria só pode ser registrada com a execução em andamento (status atual: {Status}).");
        }

        Iteracoes.Add(iteracao);
    }

    /// <summary>
    /// Conclui a execução com os resultados finais, calculando a redução de
    /// volume e a massa final (volume × densidade do material).
    /// </summary>
    public void Concluir(ResultadoOtimizacao resultado, MaterialAeroespacial material)
    {
        ArgumentNullException.ThrowIfNull(resultado);

        if (Status != StatusExecucao.Executando)
        {
            throw new RegraNegocioException(
                $"Só uma execução em andamento pode ser concluída (status atual: {Status}).");
        }

        VolumeDesignSpaceMm3 = resultado.VolumeDesignSpaceMm3;
        VolumeFinalMm3 = resultado.VolumeFinalMm3;
        EffectiveVolfrac = resultado.EffectiveVolfrac;
        Watertight = resultado.Watertight;
        GridX = resultado.GridX;
        GridY = resultado.GridY;
        GridZ = resultado.GridZ;
        NComponentsDiscarded = resultado.NComponentsDiscarded;
        SolverElapsedS = resultado.SolverElapsedS;

        ReducaoPct = resultado.VolumeDesignSpaceMm3 > 0
            ? (1.0 - resultado.VolumeFinalMm3 / resultado.VolumeDesignSpaceMm3) * 100.0
            : 0.0;

        MassaFinalKg = MaterialInfo.MassaKg(material, resultado.VolumeFinalMm3);

        Status = StatusExecucao.Concluida;
        DataFim = DateTime.UtcNow;
    }

    /// <summary>Marca a execução como falha, guardando a mensagem de erro.</summary>
    public void RegistrarFalha(string mensagem)
    {
        if (EstaFinalizada)
        {
            throw new RegraNegocioException(
                $"Execução já finalizada não pode falhar de novo (status atual: {Status}).");
        }

        Status = StatusExecucao.Erro;
        MensagemErro = string.IsNullOrWhiteSpace(mensagem) ? "Falha não especificada." : mensagem;
        DataFim = DateTime.UtcNow;
    }

    /// <summary>Cancela a execução (polite cancel), se ainda não terminou.</summary>
    public void Cancelar()
    {
        if (EstaFinalizada)
        {
            throw new RegraNegocioException(
                $"Execução já finalizada não pode ser cancelada (status atual: {Status}).");
        }

        Status = StatusExecucao.Cancelada;
        DataFim = DateTime.UtcNow;
    }

    /// <summary>
    /// Massa economizada (kg) frente à massa inicial do componente. Requer o
    /// componente carregado e a execução concluída; caso contrário retorna null.
    /// </summary>
    public decimal? MassaEconomizadaKg()
    {
        if (MassaFinalKg is null || Componente is null)
        {
            return null;
        }

        return CalculadoraLancamento.MassaEconomizadaKg(Componente.MassaInicialKg, MassaFinalKg.Value);
    }
}
