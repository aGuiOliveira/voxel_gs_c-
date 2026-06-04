using VoxelSpace.Api.Domain.Common;
using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.DTOs.Estatisticas;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Services;

/// <summary>
/// Calcula as métricas agregadas de missão a partir das execuções concluídas:
/// redução de massa, economia de custo de lançamento e rankings.
/// </summary>
public sealed class EstatisticaService : IEstatisticaService
{
    private readonly IExecucaoRepository _execucoes;

    public EstatisticaService(IExecucaoRepository execucoes)
    {
        _execucoes = execucoes;
    }

    public async Task<ResumoEstatisticasDto> ObterResumoAsync(CancellationToken cancellationToken = default)
    {
        var todas = await _execucoes.ListarAsync(cancellationToken);
        var concluidas = await _execucoes.ListarConcluidasAsync(cancellationToken);

        if (concluidas.Count == 0)
        {
            return new ResumoEstatisticasDto { TotalExecucoes = todas.Count };
        }

        var massaTotal = 0m;
        var custoTotal = 0m;
        var watertight = 0;

        // foreach explícito para acumular economia de massa/custo execução a execução.
        foreach (var execucao in concluidas)
        {
            var economia = execucao.MassaEconomizadaKg() ?? 0m;
            massaTotal += economia;
            custoTotal += CalculadoraLancamento.CustoEconomizadoUsd(economia);

            if (execucao.Watertight == true)
            {
                watertight++;
            }
        }

        return new ResumoEstatisticasDto
        {
            TotalExecucoes = todas.Count,
            TotalConcluidas = concluidas.Count,
            TaxaWatertightPct = Math.Round(100.0 * watertight / concluidas.Count, 2),
            ReducaoMediaPct = Math.Round(concluidas.Average(e => e.ReducaoPct ?? 0.0), 2),
            ReducaoMaximaPct = Math.Round(concluidas.Max(e => e.ReducaoPct ?? 0.0), 2),
            TempoMedioSolverS = Math.Round(concluidas.Average(e => e.SolverElapsedS ?? 0.0), 2),
            MassaEconomizadaTotalKg = Math.Round(massaTotal, 4),
            CustoEconomizadoTotalUsd = Math.Round(custoTotal, 2)
        };
    }

    public async Task<IReadOnlyList<EstatisticaPorMaterialDto>> ObterPorMaterialAsync(
        CancellationToken cancellationToken = default)
    {
        var concluidas = await _execucoes.ListarConcluidasAsync(cancellationToken);

        return concluidas
            .Where(e => e.Componente is not null)
            .GroupBy(e => e.Componente!.Material)
            .Select(grupo =>
            {
                var massa = grupo.Sum(e => e.MassaEconomizadaKg() ?? 0m);
                return new EstatisticaPorMaterialDto
                {
                    Material = grupo.Key,
                    MaterialNome = grupo.Key.ToString(),
                    QtdExecucoes = grupo.Count(),
                    ReducaoMediaPct = Math.Round(grupo.Average(e => e.ReducaoPct ?? 0.0), 2),
                    ReducaoMaximaPct = Math.Round(grupo.Max(e => e.ReducaoPct ?? 0.0), 2),
                    MassaEconomizadaKg = Math.Round(massa, 4),
                    CustoEconomizadoUsd = CalculadoraLancamento.CustoEconomizadoUsd(massa)
                };
            })
            .OrderByDescending(d => d.MassaEconomizadaKg)
            .ToList();
    }

    public async Task<IReadOnlyList<RankingExecucaoDto>> ObterRankingAsync(
        int top, CancellationToken cancellationToken = default)
    {
        var limite = top <= 0 ? 10 : Math.Min(top, 100);
        var concluidas = await _execucoes.ListarConcluidasAsync(cancellationToken);

        return concluidas
            .OrderByDescending(e => e.ReducaoPct ?? 0.0)
            .Take(limite)
            .Select((execucao, indice) => MapearRanking(execucao, indice + 1))
            .ToList();
    }

    private static RankingExecucaoDto MapearRanking(ExecucaoOtimizacao execucao, int posicao)
    {
        var massa = execucao.MassaEconomizadaKg() ?? 0m;
        return new RankingExecucaoDto
        {
            Posicao = posicao,
            ExecucaoId = execucao.Id,
            ComponenteNome = execucao.Componente?.Nome ?? "(desconhecido)",
            Material = execucao.Componente?.Material ?? default,
            ReducaoPct = Math.Round(execucao.ReducaoPct ?? 0.0, 2),
            MassaEconomizadaKg = Math.Round(massa, 4),
            CustoEconomizadoUsd = CalculadoraLancamento.CustoEconomizadoUsd(massa)
        };
    }
}
