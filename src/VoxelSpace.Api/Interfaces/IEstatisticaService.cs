using VoxelSpace.Api.DTOs.Estatisticas;

namespace VoxelSpace.Api.Interfaces;

/// <summary>
/// Serviço de analytics: transforma as execuções concluídas em métricas de
/// missão (redução de massa, economia de custo de lançamento, rankings).
/// </summary>
public interface IEstatisticaService
{
    Task<ResumoEstatisticasDto> ObterResumoAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EstatisticaPorMaterialDto>> ObterPorMaterialAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RankingExecucaoDto>> ObterRankingAsync(int top, CancellationToken cancellationToken = default);
}
