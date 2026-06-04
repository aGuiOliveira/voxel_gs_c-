using Microsoft.AspNetCore.Mvc;
using VoxelSpace.Api.DTOs.Estatisticas;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Controllers;

/// <summary>
/// Analytics de missão: redução de massa, economia de custo de lançamento e
/// rankings, derivados das execuções concluídas.
/// </summary>
[ApiController]
[Route("api/estatisticas")]
[Produces("application/json")]
public sealed class EstatisticasController : ControllerBase
{
    private readonly IEstatisticaService _estatisticas;

    public EstatisticasController(IEstatisticaService estatisticas)
    {
        _estatisticas = estatisticas;
    }

    /// <summary>Resumo global: reduções, taxa de sucesso e custo de lançamento economizado.</summary>
    [HttpGet("resumo")]
    public async Task<ActionResult<ResumoEstatisticasDto>> Resumo(CancellationToken ct)
        => Ok(await _estatisticas.ObterResumoAsync(ct));

    /// <summary>Redução e economia agregadas por material aeroespacial.</summary>
    [HttpGet("por-material")]
    public async Task<ActionResult<IEnumerable<EstatisticaPorMaterialDto>>> PorMaterial(CancellationToken ct)
        => Ok(await _estatisticas.ObterPorMaterialAsync(ct));

    /// <summary>Ranking das execuções por percentual de redução de massa.</summary>
    [HttpGet("ranking")]
    public async Task<ActionResult<IEnumerable<RankingExecucaoDto>>> Ranking(
        [FromQuery] int top = 10, CancellationToken ct = default)
        => Ok(await _estatisticas.ObterRankingAsync(top, ct));
}
