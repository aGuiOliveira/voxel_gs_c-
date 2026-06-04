namespace VoxelSpace.Api.Interfaces;

/// <summary>
/// Contrato genérico de repositório. Abstrai o acesso a dados (EF Core) das
/// camadas de serviço/controller — estas dependem da interface, não da
/// implementação concreta (inversão de dependência).
/// </summary>
/// <typeparam name="T">Tipo de entidade gerenciada.</typeparam>
public interface IRepositorio<T> where T : class
{
    Task<T?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListarAsync(CancellationToken cancellationToken = default);

    Task AdicionarAsync(T entidade, CancellationToken cancellationToken = default);

    void Atualizar(T entidade);

    void Remover(T entidade);

    Task<int> SalvarAsync(CancellationToken cancellationToken = default);
}
