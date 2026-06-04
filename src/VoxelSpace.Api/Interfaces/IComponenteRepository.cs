using VoxelSpace.Api.Domain.Componentes;

namespace VoxelSpace.Api.Interfaces;

/// <summary>
/// Repositório de componentes espaciais, com a consulta detalhada que traz as
/// execuções associadas.
/// </summary>
public interface IComponenteRepository : IRepositorio<ComponenteEspacial>
{
    /// <summary>Componente com suas execuções carregadas (ou null).</summary>
    Task<ComponenteEspacial?> ObterComExecucoesAsync(int id, CancellationToken cancellationToken = default);
}
