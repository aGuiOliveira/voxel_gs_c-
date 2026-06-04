namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Discriminador usado na API para criar o componente concreto correto
/// (a hierarquia em si é resolvida por TPH no banco).
/// </summary>
public enum TipoComponente
{
    SuporteEstrutural = 0,
    PainelSolar = 1,
    SuporteAntena = 2
}
