using System.ComponentModel.DataAnnotations;
using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.DTOs.Componentes;

/// <summary>
/// Dados para criar um componente. O campo <see cref="Tipo"/> escolhe a subclasse
/// concreta; os campos específicos só são lidos para o tipo correspondente.
/// </summary>
public sealed class CriarComponenteDto
{
    [Required]
    public TipoComponente Tipo { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public MaterialAeroespacial Material { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "MassaInicialKg não pode ser negativa.")]
    public decimal MassaInicialKg { get; set; }

    // Específico de SuporteEstrutural
    public double? CargaSuportadaKn { get; set; }

    // Específico de PainelSolar
    public double? AreaM2 { get; set; }
    public double? PotenciaW { get; set; }

    // Específico de SuporteAntena
    public double? FrequenciaGhz { get; set; }
}
