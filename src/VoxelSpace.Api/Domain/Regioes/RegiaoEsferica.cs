namespace VoxelSpace.Api.Domain.Regioes;

/// <summary>Região esférica definida por centro e raio (mm).</summary>
public sealed class RegiaoEsferica : RegiaoFronteira
{
    public double CentroX { get; set; }
    public double CentroY { get; set; }
    public double CentroZ { get; set; }
    public double Raio { get; set; }

    public override double CalcularVolumeMm3() => 4.0 / 3.0 * Math.PI * Math.Pow(Raio, 3);

    public override string DescreverGeometria() =>
        $"Esfera r={Raio:0.##} em ({CentroX:0.##}, {CentroY:0.##}, {CentroZ:0.##})";
}
