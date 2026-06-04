namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Papel de uma região de contorno na otimização.
/// </summary>
public enum TipoCondicao
{
    /// <summary>Apoio: DOFs fixos no FEM (a peça é presa aqui).</summary>
    Apoio = 0,

    /// <summary>Força: carga aplicada com vetor (Fx, Fy, Fz).</summary>
    Forca = 1,

    /// <summary>Keep-solid: material que nunca pode ser removido pelo otimizador.</summary>
    KeepSolid = 2
}
