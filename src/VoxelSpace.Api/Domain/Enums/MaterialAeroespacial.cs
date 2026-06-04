namespace VoxelSpace.Api.Domain.Enums;

/// <summary>
/// Materiais estruturais típicos de hardware espacial. A densidade de cada um
/// é resolvida em <see cref="Common.MaterialInfo"/> para converter volume (mm³)
/// em massa (kg).
/// </summary>
public enum MaterialAeroespacial
{
    /// <summary>Liga de alumínio 7075-T6 — leve, usada em estruturas primárias.</summary>
    Aluminio7075 = 0,

    /// <summary>Liga de titânio Ti-6Al-4V — alta resistência, usada em fixações críticas.</summary>
    Titanio6Al4V = 1,

    /// <summary>Compósito de fibra de carbono — painéis e tubos de baixa massa.</summary>
    FibraCarbono = 2,

    /// <summary>Superliga Inconel 718 — componentes expostos a alta temperatura.</summary>
    Inconel718 = 3
}
