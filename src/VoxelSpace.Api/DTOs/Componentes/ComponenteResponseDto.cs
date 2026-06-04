using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Domain.Common;
using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.DTOs.Componentes;

/// <summary>Representação de saída de um componente espacial.</summary>
public sealed record ComponenteResponseDto
{
    public int Id { get; init; }
    public string Tipo { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public MaterialAeroespacial Material { get; init; }
    public string MaterialNome { get; init; } = string.Empty;
    public decimal MassaInicialKg { get; init; }
    public DateTime DataCadastroUtc { get; init; }
    public DateTime DataCadastroLocal { get; init; }
    public decimal IndiceCriticidade { get; init; }
    public string ClassificacaoMissao { get; init; } = string.Empty;
    public string ResumoTecnico { get; init; } = string.Empty;
    public int QtdExecucoes { get; init; }

    // Campos específicos por subtipo (preenchidos conforme o tipo).
    public double? CargaSuportadaKn { get; init; }
    public double? AreaM2 { get; init; }
    public double? PotenciaW { get; init; }
    public double? FrequenciaGhz { get; init; }

    /// <summary>Mapeia uma entidade de domínio para o DTO de resposta.</summary>
    public static ComponenteResponseDto De(ComponenteEspacial componente)
    {
        var dto = new ComponenteResponseDto
        {
            Id = componente.Id,
            Tipo = componente.GetType().Name,
            Nome = componente.Nome,
            Material = componente.Material,
            MaterialNome = componente.Material.ToString(),
            MassaInicialKg = componente.MassaInicialKg,
            DataCadastroUtc = componente.DataCadastro,
            DataCadastroLocal = FusoHorario.ParaBrasilia(componente.DataCadastro),
            IndiceCriticidade = componente.CalcularIndiceCriticidade(),
            ClassificacaoMissao = componente.ClassificacaoMissao(),
            ResumoTecnico = componente.ResumoTecnico(),
            QtdExecucoes = componente.Execucoes?.Count ?? 0
        };

        // switch sobre o tipo concreto para projetar os campos específicos.
        return componente switch
        {
            SuporteEstrutural s => dto with { CargaSuportadaKn = s.CargaSuportadaKn },
            PainelSolar p => dto with { AreaM2 = p.AreaM2, PotenciaW = p.PotenciaW },
            SuporteAntena a => dto with { FrequenciaGhz = a.FrequenciaGhz },
            _ => dto
        };
    }
}
