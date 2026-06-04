namespace VoxelSpace.Api.Domain.Regioes;

/// <summary>Região em caixa (AABB) definida por dois cantos opostos (mm).</summary>
public sealed class RegiaoCaixa : RegiaoFronteira
{
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double MinZ { get; set; }
    public double MaxX { get; set; }
    public double MaxY { get; set; }
    public double MaxZ { get; set; }

    public override double CalcularVolumeMm3()
    {
        var dx = Math.Abs(MaxX - MinX);
        var dy = Math.Abs(MaxY - MinY);
        var dz = Math.Abs(MaxZ - MinZ);
        return dx * dy * dz;
    }

    public override string DescreverGeometria() =>
        $"Caixa [{MinX:0.##},{MinY:0.##},{MinZ:0.##}]→[{MaxX:0.##},{MaxY:0.##},{MaxZ:0.##}]";
}
