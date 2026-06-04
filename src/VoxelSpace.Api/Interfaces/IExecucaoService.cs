using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.Domain.Regioes;

namespace VoxelSpace.Api.Interfaces;

/// <summary>
/// Orquestra o ciclo de vida de uma execução de otimização, aplicando as regras
/// de negócio (validação de parâmetros, transições de estado, cálculo de massa).
/// </summary>
public interface IExecucaoService
{
    Task<ExecucaoOtimizacao> CriarAsync(
        int componenteId, ParametrosOtimizacao parametros, CancellationToken cancellationToken = default);

    Task<ExecucaoOtimizacao> IniciarAsync(int id, CancellationToken cancellationToken = default);

    Task<ExecucaoOtimizacao> ConcluirAsync(
        int id, ResultadoOtimizacao resultado, CancellationToken cancellationToken = default);

    Task<ExecucaoOtimizacao> CancelarAsync(int id, CancellationToken cancellationToken = default);

    Task<IteracaoOtimizacao> RegistrarIteracaoAsync(
        int id, IteracaoOtimizacao iteracao, CancellationToken cancellationToken = default);

    Task<RegiaoFronteira> AdicionarRegiaoAsync(
        int id, RegiaoFronteira regiao, CancellationToken cancellationToken = default);
}
