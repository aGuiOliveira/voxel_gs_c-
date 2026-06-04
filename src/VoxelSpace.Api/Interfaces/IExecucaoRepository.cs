using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.Interfaces;

/// <summary>
/// Repositório de execuções de otimização, com filtros por status e por janela
/// temporal, além das consultas que carregam as entidades relacionadas.
/// </summary>
public interface IExecucaoRepository : IRepositorio<ExecucaoOtimizacao>
{
    /// <summary>
    /// Lista execuções aplicando filtros opcionais de status e de intervalo de
    /// datas (sobre DataCriacao). Datas em UTC.
    /// </summary>
    Task<IReadOnlyList<ExecucaoOtimizacao>> ListarFiltradoAsync(
        StatusExecucao? status,
        DateTime? de,
        DateTime? ate,
        CancellationToken cancellationToken = default);

    /// <summary>Execução com componente, parâmetros e regiões carregados.</summary>
    Task<ExecucaoOtimizacao?> ObterDetalhadoAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Execução com a coleção de iterações carregada (curva de convergência).</summary>
    Task<ExecucaoOtimizacao?> ObterComIteracoesAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Execução com a coleção de regiões carregada.</summary>
    Task<ExecucaoOtimizacao?> ObterComRegioesAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Todas as execuções concluídas, com componente, para estatísticas.</summary>
    Task<IReadOnlyList<ExecucaoOtimizacao>> ListarConcluidasAsync(CancellationToken cancellationToken = default);
}
