namespace VoxelSpace.Api.Exceptions;

/// <summary>
/// Lançada quando um parâmetro de entrada é inválido (fora de faixa, mal formado).
/// Mapeada para HTTP 400 (Bad Request).
/// </summary>
public sealed class ParametroInvalidoException : Exception
{
    public ParametroInvalidoException(string mensagem) : base(mensagem)
    {
    }

    public ParametroInvalidoException(string mensagem, Exception innerException)
        : base(mensagem, innerException)
    {
    }
}
