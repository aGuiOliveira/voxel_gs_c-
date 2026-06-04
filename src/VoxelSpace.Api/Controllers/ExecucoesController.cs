using Microsoft.AspNetCore.Mvc;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.DTOs.Execucoes;
using VoxelSpace.Api.DTOs.Iteracoes;
using VoxelSpace.Api.DTOs.Regioes;
using VoxelSpace.Api.Exceptions;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Controllers;

/// <summary>
/// Execuções de otimização: criação, consulta com filtros, transições de estado,
/// telemetria de iterações e condições de contorno.
/// </summary>
[ApiController]
[Route("api/execucoes")]
[Produces("application/json")]
public sealed class ExecucoesController : ControllerBase
{
    private readonly IExecucaoRepository _execucoes;
    private readonly IExecucaoService _servico;

    public ExecucoesController(IExecucaoRepository execucoes, IExecucaoService servico)
    {
        _execucoes = execucoes;
        _servico = servico;
    }

    /// <summary>
    /// Lista execuções, com filtros opcionais por status e por janela temporal
    /// (?status=Concluida&amp;de=2026-01-01&amp;ate=2026-12-31). Datas tratadas como UTC.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExecucaoResponseDto>>> Listar(
        [FromQuery] StatusExecucao? status,
        [FromQuery] DateTime? de,
        [FromQuery] DateTime? ate,
        CancellationToken ct)
    {
        if (de.HasValue && ate.HasValue && de.Value > ate.Value)
        {
            throw new ParametroInvalidoException("'de' não pode ser posterior a 'ate'.");
        }

        var deUtc = NormalizarUtc(de);
        var ateUtc = NormalizarUtc(ate);

        var execucoes = await _execucoes.ListarFiltradoAsync(status, deUtc, ateUtc, ct);
        return Ok(execucoes.Select(ExecucaoResponseDto.De));
    }

    /// <summary>Detalhe de uma execução (com componente e regiões).</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExecucaoResponseDto>> Obter(int id, CancellationToken ct)
    {
        var execucao = await _execucoes.ObterDetalhadoAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Execução", id);
        return Ok(ExecucaoResponseDto.De(execucao));
    }

    /// <summary>Cria uma execução de otimização para um componente.</summary>
    [HttpPost]
    public async Task<ActionResult<ExecucaoResponseDto>> Criar(
        [FromBody] CriarExecucaoDto dto, CancellationToken ct)
    {
        var execucao = await _servico.CriarAsync(dto.ComponenteId, dto.Parametros.ParaDominio(), ct);
        var resposta = ExecucaoResponseDto.De(execucao);
        return CreatedAtAction(nameof(Obter), new { id = execucao.Id }, resposta);
    }

    /// <summary>Inicia a execução (Enfileirada → Executando).</summary>
    [HttpPost("{id:int}/iniciar")]
    public async Task<ActionResult<ExecucaoResponseDto>> Iniciar(int id, CancellationToken ct)
        => Ok(ExecucaoResponseDto.De(await _servico.IniciarAsync(id, ct)));

    /// <summary>Conclui a execução com os resultados finais (Executando → Concluida).</summary>
    [HttpPost("{id:int}/concluir")]
    public async Task<ActionResult<ExecucaoResponseDto>> Concluir(
        int id, [FromBody] ConcluirExecucaoDto dto, CancellationToken ct)
        => Ok(ExecucaoResponseDto.De(await _servico.ConcluirAsync(id, dto.ParaDominio(), ct)));

    /// <summary>Cancela a execução (polite cancel).</summary>
    [HttpPost("{id:int}/cancelar")]
    public async Task<ActionResult<ExecucaoResponseDto>> Cancelar(int id, CancellationToken ct)
        => Ok(ExecucaoResponseDto.De(await _servico.CancelarAsync(id, ct)));

    // ---------------- iterações (telemetria / curva de convergência) ----------------

    /// <summary>Curva de convergência: todas as iterações registradas, em ordem.</summary>
    [HttpGet("{id:int}/iteracoes")]
    public async Task<ActionResult<IEnumerable<IteracaoResponseDto>>> ListarIteracoes(int id, CancellationToken ct)
    {
        var execucao = await _execucoes.ObterComIteracoesAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Execução", id);
        return Ok(execucao.Iteracoes.Select(IteracaoResponseDto.De));
    }

    /// <summary>Registra uma amostra de telemetria de iteração.</summary>
    [HttpPost("{id:int}/iteracoes")]
    public async Task<ActionResult<IteracaoResponseDto>> RegistrarIteracao(
        int id, [FromBody] RegistrarIteracaoDto dto, CancellationToken ct)
    {
        var iteracao = await _servico.RegistrarIteracaoAsync(id, dto.ParaDominio(), ct);
        return Ok(IteracaoResponseDto.De(iteracao));
    }

    // ---------------- regiões (condições de contorno) ----------------

    /// <summary>Lista as condições de contorno (apoios, forças, keep-solid) da execução.</summary>
    [HttpGet("{id:int}/regioes")]
    public async Task<ActionResult<IEnumerable<RegiaoResponseDto>>> ListarRegioes(int id, CancellationToken ct)
    {
        var execucao = await _execucoes.ObterComRegioesAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Execução", id);
        return Ok(execucao.Regioes.Select(RegiaoResponseDto.De));
    }

    /// <summary>Adiciona uma condição de contorno à execução.</summary>
    [HttpPost("{id:int}/regioes")]
    public async Task<ActionResult<RegiaoResponseDto>> AdicionarRegiao(
        int id, [FromBody] CriarRegiaoDto dto, CancellationToken ct)
    {
        var regiao = await _servico.AdicionarRegiaoAsync(id, dto.ParaDominio(), ct);
        return Ok(RegiaoResponseDto.De(regiao));
    }

    /// <summary>Garante que a data filtrada seja interpretada como UTC.</summary>
    private static DateTime? NormalizarUtc(DateTime? data)
    {
        if (!data.HasValue)
        {
            return null;
        }

        return data.Value.Kind switch
        {
            DateTimeKind.Utc => data,
            DateTimeKind.Local => data.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(data.Value, DateTimeKind.Utc)
        };
    }
}
