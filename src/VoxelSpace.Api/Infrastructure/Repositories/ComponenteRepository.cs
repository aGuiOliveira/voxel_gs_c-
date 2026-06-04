using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Infrastructure.Data;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Infrastructure.Repositories;

public sealed class ComponenteRepository : Repositorio<ComponenteEspacial>, IComponenteRepository
{
    public ComponenteRepository(VoxelDbContext contexto) : base(contexto)
    {
    }

    public async Task<ComponenteEspacial?> ObterComExecucoesAsync(
        int id, CancellationToken cancellationToken = default)
        => await Contexto.Componentes
            .Include(c => c.Execucoes)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}
