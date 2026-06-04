namespace VoxelSpace.Api.Exceptions;

/// <summary>
/// Lançada quando um recurso referenciado por id não existe. Mapeada para HTTP 404
/// pelo middleware de exceções.
/// </summary>
public sealed class RecursoNaoEncontradoException : Exception
{
    public RecursoNaoEncontradoException(string recurso, object id)
        : base($"{recurso} de id '{id}' não foi encontrado.")
    {
    }

    public RecursoNaoEncontradoException(string mensagem) : base(mensagem)
    {
    }
}
