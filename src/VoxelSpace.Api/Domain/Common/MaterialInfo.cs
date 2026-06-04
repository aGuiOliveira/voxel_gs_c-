using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.Domain.Common;

/// <summary>
/// Tabela estática de propriedades de materiais aeroespaciais. Centraliza as
/// densidades para converter volume geométrico (resultado da otimização, em mm³)
/// em massa (kg) — a grandeza que realmente importa para o custo de lançamento.
/// </summary>
public static class MaterialInfo
{
    // Densidades em kg/mm³ (g/cm³ ÷ 1_000_000). Fonte: valores nominais de catálogo.
    private static readonly IReadOnlyDictionary<MaterialAeroespacial, decimal> DensidadesKgPorMm3 =
        new Dictionary<MaterialAeroespacial, decimal>
        {
            [MaterialAeroespacial.Aluminio7075] = 0.000_002_810m, // 2.81 g/cm³
            [MaterialAeroespacial.Titanio6Al4V] = 0.000_004_430m, // 4.43 g/cm³
            [MaterialAeroespacial.FibraCarbono] = 0.000_001_600m, // 1.60 g/cm³
            [MaterialAeroespacial.Inconel718]   = 0.000_008_190m, // 8.19 g/cm³
        };

    /// <summary>
    /// Densidade do material em kg/mm³.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Material não catalogado.</exception>
    public static decimal DensidadeKgPorMm3(MaterialAeroespacial material)
    {
        if (!DensidadesKgPorMm3.TryGetValue(material, out var densidade))
        {
            throw new ArgumentOutOfRangeException(
                nameof(material), material, "Material aeroespacial sem densidade cadastrada.");
        }

        return densidade;
    }

    /// <summary>
    /// Converte um volume em mm³ na massa correspondente (kg) para o material.
    /// </summary>
    public static decimal MassaKg(MaterialAeroespacial material, double volumeMm3)
    {
        if (volumeMm3 < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(volumeMm3), volumeMm3, "Volume não pode ser negativo.");
        }

        return DensidadeKgPorMm3(material) * (decimal)volumeMm3;
    }
}
