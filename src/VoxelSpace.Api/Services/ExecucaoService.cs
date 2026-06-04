using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.Domain.Regioes;
using VoxelSpace.Api.Exceptions;
using VoxelSpace.Api.Interfaces;

namespace VoxelSpace.Api.Services;

/// <summary>
/// Implementa as regras de negócio do ciclo de vida de uma execução. Cada método
/// tem uma responsabilidade única (criar, iniciar, concluir, cancelar, registrar
/// telemetria, anexar região) e traduz falhas técnicas (banco) em exceções de
/// domínio claras, sem deixar a aplicação cair.
/// </summary>
public sealed class ExecucaoService : IExecucaoService
{
    private readonly IExecucaoRepository _execucoes;
    private readonly IComponenteRepository _componentes;
    private readonly ILogger<ExecucaoService> _logger;

    public ExecucaoService(
        IExecucaoRepository execucoes,
        IComponenteRepository componentes,
        ILogger<ExecucaoService> logger)
    {
        _execucoes = execucoes;
        _componentes = componentes;
        _logger = logger;
    }

    public async Task<ExecucaoOtimizacao> CriarAsync(
        int componenteId, ParametrosOtimizacao parametros, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parametros);
        parametros.Validar();

        var componente = await _componentes.ObterPorIdAsync(componenteId, cancellationToken)
            ?? throw new RecursoNaoEncontradoException("Componente", componenteId);

        var execucao = new ExecucaoOtimizacao
        {
            ComponenteId = componente.Id,
            Parametros = parametros
        };

        await _execucoes.AdicionarAsync(execucao, cancellationToken);
        await SalvarComTratamentoAsync(cancellationToken);

        _logger.LogInformation(
            "Execução {ExecucaoId} criada para o componente {ComponenteId}.", execucao.Id, componente.Id);
        return execucao;
    }

    public async Task<ExecucaoOtimizacao> IniciarAsync(int id, CancellationToken cancellationToken = default)
    {
        var execucao = await ObterRastreadaAsync(id, cancellationToken);
        execucao.Iniciar();
        await SalvarComTratamentoAsync(cancellationToken);
        return execucao;
    }

    public async Task<ExecucaoOtimizacao> ConcluirAsync(
        int id, ResultadoOtimizacao resultado, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resultado);

        var execucao = await _execucoes.ObterDetalhadoAsync(id, cancellationToken)
            ?? throw new RecursoNaoEncontradoException("Execução", id);

        if (execucao.Componente is null)
        {
            throw new RegraNegocioException("Execução sem componente associado — não é possível calcular a massa.");
        }

        execucao.Concluir(resultado, execucao.Componente.Material);
        await SalvarComTratamentoAsync(cancellationToken);

        _logger.LogInformation(
            "Execução {ExecucaoId} concluída: redução {Reducao:0.##}%.", execucao.Id, execucao.ReducaoPct);
        return execucao;
    }

    public async Task<ExecucaoOtimizacao> CancelarAsync(int id, CancellationToken cancellationToken = default)
    {
        var execucao = await ObterRastreadaAsync(id, cancellationToken);
        execucao.Cancelar();
        await SalvarComTratamentoAsync(cancellationToken);
        return execucao;
    }

    public async Task<IteracaoOtimizacao> RegistrarIteracaoAsync(
        int id, IteracaoOtimizacao iteracao, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(iteracao);

        var execucao = await ObterRastreadaAsync(id, cancellationToken);
        iteracao.ExecucaoId = execucao.Id;
        execucao.RegistrarIteracao(iteracao);
        await SalvarComTratamentoAsync(cancellationToken);
        return iteracao;
    }

    public async Task<RegiaoFronteira> AdicionarRegiaoAsync(
        int id, RegiaoFronteira regiao, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(regiao);

        var execucao = await ObterRastreadaAsync(id, cancellationToken);

        if (execucao.EstaFinalizada)
        {
            throw new RegraNegocioException(
                $"Não é possível adicionar condições de contorno a uma execução finalizada (status: {execucao.Status}).");
        }

        regiao.ExecucaoId = execucao.Id;
        execucao.Regioes.Add(regiao);
        await SalvarComTratamentoAsync(cancellationToken);
        return regiao;
    }

    private async Task<ExecucaoOtimizacao> ObterRastreadaAsync(int id, CancellationToken cancellationToken)
        => await _execucoes.ObterPorIdAsync(id, cancellationToken)
            ?? throw new RecursoNaoEncontradoException("Execução", id);

    /// <summary>
    /// Persiste capturando falhas técnicas específicas do banco e reembrulhando-as
    /// em uma exceção de domínio com mensagem clara — a API responde 409/400 em vez
    /// de derrubar o processo.
    /// </summary>
    private async Task SalvarComTratamentoAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _execucoes.SalvarAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Conflito de concorrência ao salvar execução.");
            throw new RegraNegocioException(
                "O registro foi modificado por outra operação. Recarregue e tente novamente.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Falha ao persistir execução no banco.");
            throw new RegraNegocioException(
                "Não foi possível salvar a execução (violação de integridade no banco).");
        }
    }
}
