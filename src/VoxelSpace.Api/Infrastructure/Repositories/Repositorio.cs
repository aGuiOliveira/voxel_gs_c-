using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Infrastructure.Data;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Infrastructure.Repositories;

/// <summary>
/// Implementação genérica de <see cref="IRepositorio{T}"/> sobre EF Core.
/// As subclasses adicionam consultas específicas do agregado.
/// </summary>
public class Repositorio<T> : IRepositorio<T> where T : class
{
    protected readonly VoxelDbContext Contexto;

    public Repositorio(VoxelDbContext contexto)
    {
        Contexto = contexto;
    }

    public virtual async Task<T?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        => await Contexto.Set<T>().FindAsync([id], cancellationToken);

    public virtual async Task<IReadOnlyList<T>> ListarAsync(CancellationToken cancellationToken = default)
        => await Contexto.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public async Task AdicionarAsync(T entidade, CancellationToken cancellationToken = default)
        => await Contexto.Set<T>().AddAsync(entidade, cancellationToken);

    public void Atualizar(T entidade) => Contexto.Set<T>().Update(entidade);

    public void Remover(T entidade) => Contexto.Set<T>().Remove(entidade);

    public Task<int> SalvarAsync(CancellationToken cancellationToken = default)
        => Contexto.SaveChangesAsync(cancellationToken);
}
