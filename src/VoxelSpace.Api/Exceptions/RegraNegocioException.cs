namespace VoxelSpace.Api.Exceptions;

/// <summary>
/// Lançada quando uma operação viola uma regra de negócio (ex.: transição de
/// estado inválida de uma execução). Mapeada para HTTP 409 (Conflict).
/// </summary>
public sealed class RegraNegocioException : Exception
{
    public RegraNegocioException(string mensagem) : base(mensagem)
    {
    }
}
