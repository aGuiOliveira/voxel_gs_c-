using VoxelSpace.Api.Domain.Enums;

namespace VoxelSpace.Api.Domain.Regioes;

/// <summary>
/// Região correspondente a uma face inteira do bounding box do espaço de projeto,
/// com uma espessura para dentro. É o jeito mais comum de prender um cantilever
/// (apoio na face x_min) ou aplicar carga distribuída.
/// </summary>
public sealed class RegiaoFace : RegiaoFronteira
{
    /// <summary>Qual face do bounding box.</summary>
    public FaceGrid Face { get; set; }

    /// <summary>Espessura da camada a partir da face, em mm.</summary>
    public double EspessuraMm { get; set; } = 1.0;

    /// <summary>Área da face do bounding box, em mm² (usada para estimar o volume).</summary>
    public double AreaFaceMm2 { get; set; }

    public override double CalcularVolumeMm3() => AreaFaceMm2 * EspessuraMm;

    public override string DescreverGeometria() =>
        $"Face {Face} (espessura {EspessuraMm:0.##} mm)";
}
