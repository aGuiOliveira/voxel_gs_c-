using System.ComponentModel.DataAnnotations;

namespace VoxelSpace.Api.DTOs.Execucoes;

/// <summary>Dados para criar uma execução de otimização para um componente.</summary>
public sealed class CriarExecucaoDto
{
    [Range(1, int.MaxValue, ErrorMessage = "ComponenteId deve ser um id válido.")]
    public int ComponenteId { get; set; }

    [Required]
    public ParametrosDto Parametros { get; set; } = new();
}
