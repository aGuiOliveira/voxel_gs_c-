namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Ciclo de vida de uma execução de otimização topológica.
/// Espelha os estados do JobManager do engine (queued/running/done/error/cancelled).
/// </summary>
public enum StatusExecucao
{
    Enfileirada = 0,
    Executando = 1,
    Concluida = 2,
    Erro = 3,
    Cancelada = 4
}
