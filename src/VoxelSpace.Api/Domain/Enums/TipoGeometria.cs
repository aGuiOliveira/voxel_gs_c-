namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Discriminador de geometria de uma região de contorno na criação via API.
/// </summary>
public enum TipoGeometria
{
    Esfera = 0,
    Caixa = 1,
    Face = 2
}
