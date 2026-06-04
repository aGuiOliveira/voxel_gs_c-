using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.DTOs.Componentes;
using VoxelSpace.Api.Exceptions;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Controllers;

/// <summary>CRUD de componentes espaciais (suportes, painéis, antenas).</summary>
[ApiController]
[Route("api/componentes")]
[Produces("application/json")]
public sealed class ComponentesController : ControllerBase
{
    private readonly IComponenteRepository _componentes;
    private readonly ILogger<ComponentesController> _logger;

    public ComponentesController(IComponenteRepository componentes, ILogger<ComponentesController> logger)
    {
        _componentes = componentes;
        _logger = logger;
    }

    /// <summary>Lista todos os componentes cadastrados.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComponenteResponseDto>>> Listar(CancellationToken ct)
    {
        var componentes = await _componentes.ListarAsync(ct);
        return Ok(componentes.Select(ComponenteResponseDto.De));
    }

    /// <summary>Obtém um componente com seu resumo técnico.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ComponenteResponseDto>> Obter(int id, CancellationToken ct)
    {
        var componente = await _componentes.ObterComExecucoesAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Componente", id);
        return Ok(ComponenteResponseDto.De(componente));
    }

    /// <summary>Cria um novo componente do tipo informado.</summary>
    [HttpPost]
    public async Task<ActionResult<ComponenteResponseDto>> Criar(
        [FromBody] CriarComponenteDto dto, CancellationToken ct)
    {
        var componente = ConstruirComponente(dto);

        try
        {
            await _componentes.AdicionarAsync(componente, ct);
            await _componentes.SalvarAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Falha ao salvar componente '{Nome}'.", dto.Nome);
            throw new RegraNegocioException("Não foi possível salvar o componente (violação de integridade no banco).");
        }

        var resposta = ComponenteResponseDto.De(componente);
        return CreatedAtAction(nameof(Obter), new { id = componente.Id }, resposta);
    }

    /// <summary>Atualiza os dados comuns e específicos de um componente.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ComponenteResponseDto>> Atualizar(
        int id, [FromBody] CriarComponenteDto dto, CancellationToken ct)
    {
        var componente = await _componentes.ObterPorIdAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Componente", id);

        componente.Nome = dto.Nome;
        componente.Material = dto.Material;
        componente.MassaInicialKg = dto.MassaInicialKg;
        AplicarCamposEspecificos(componente, dto);

        _componentes.Atualizar(componente);
        await _componentes.SalvarAsync(ct);
        return Ok(ComponenteResponseDto.De(componente));
    }

    /// <summary>Remove um componente (e suas execuções, em cascata).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var componente = await _componentes.ObterPorIdAsync(id, ct)
            ?? throw new RecursoNaoEncontradoException("Componente", id);

        _componentes.Remover(componente);
        await _componentes.SalvarAsync(ct);
        return NoContent();
    }

    /// <summary>Fabrica a subclasse concreta conforme o tipo do DTO.</summary>
    private static ComponenteEspacial ConstruirComponente(CriarComponenteDto dto)
    {
        ComponenteEspacial componente = dto.Tipo switch
        {
            TipoComponente.SuporteEstrutural => new SuporteEstrutural
            {
                CargaSuportadaKn = dto.CargaSuportadaKn ?? 0.0
            },
            TipoComponente.PainelSolar => new PainelSolar
            {
                AreaM2 = dto.AreaM2 ?? 0.0,
                PotenciaW = dto.PotenciaW ?? 0.0
            },
            TipoComponente.SuporteAntena => new SuporteAntena
            {
                FrequenciaGhz = dto.FrequenciaGhz ?? 0.0
            },
            _ => throw new ParametroInvalidoException($"Tipo de componente desconhecido: {dto.Tipo}.")
        };

        componente.Nome = dto.Nome;
        componente.Material = dto.Material;
        componente.MassaInicialKg = dto.MassaInicialKg;
        return componente;
    }

    /// <summary>Atualiza campos específicos quando o tipo concreto bate com o DTO.</summary>
    private static void AplicarCamposEspecificos(ComponenteEspacial componente, CriarComponenteDto dto)
    {
        switch (componente)
        {
            case SuporteEstrutural s when dto.CargaSuportadaKn.HasValue:
                s.CargaSuportadaKn = dto.CargaSuportadaKn.Value;
                break;
            case PainelSolar p:
                if (dto.AreaM2.HasValue) p.AreaM2 = dto.AreaM2.Value;
                if (dto.PotenciaW.HasValue) p.PotenciaW = dto.PotenciaW.Value;
                break;
            case SuporteAntena a when dto.FrequenciaGhz.HasValue:
                a.FrequenciaGhz = dto.FrequenciaGhz.Value;
                break;
        }
    }
}
