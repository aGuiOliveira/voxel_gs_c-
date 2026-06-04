using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.Infrastructure.Data;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Infrastructure.Repositories;

public sealed class ExecucaoRepository : Repositorio<ExecucaoOtimizacao>, IExecucaoRepository
{
    public ExecucaoRepository(VoxelDbContext contexto) : base(contexto)
    {
    }

    public override async Task<IReadOnlyList<ExecucaoOtimizacao>> ListarAsync(
        CancellationToken cancellationToken = default)
        => await Contexto.Execucoes
            .AsNoTracking()
            .Include(x => x.Componente)
            .OrderByDescending(x => x.DataCriacao)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ExecucaoOtimizacao>> ListarFiltradoAsync(
        StatusExecucao? status,
        DateTime? de,
        DateTime? ate,
        CancellationToken cancellationToken = default)
    {
        var query = Contexto.Execucoes
            .AsNoTracking()
            .Include(x => x.Componente)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        if (de.HasValue)
        {
            query = query.Where(x => x.DataCriacao >= de.Value);
        }

        if (ate.HasValue)
        {
            query = query.Where(x => x.DataCriacao <= ate.Value);
        }

        return await query
            .OrderByDescending(x => x.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExecucaoOtimizacao?> ObterDetalhadoAsync(
        int id, CancellationToken cancellationToken = default)
        => await Contexto.Execucoes
            .Include(x => x.Componente)
            .Include(x => x.Regioes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ExecucaoOtimizacao?> ObterComIteracoesAsync(
        int id, CancellationToken cancellationToken = default)
        => await Contexto.Execucoes
            .Include(x => x.Iteracoes.OrderBy(i => i.NumeroIteracao))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<ExecucaoOtimizacao?> ObterComRegioesAsync(
        int id, CancellationToken cancellationToken = default)
        => await Contexto.Execucoes
            .Include(x => x.Regioes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ExecucaoOtimizacao>> ListarConcluidasAsync(
        CancellationToken cancellationToken = default)
        => await Contexto.Execucoes
            .AsNoTracking()
            .Include(x => x.Componente)
            .Where(x => x.Status == StatusExecucao.Concluida)
            .ToListAsync(cancellationToken);
}
