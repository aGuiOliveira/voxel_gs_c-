using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.DTOs.Execucoes;

/// <summary>Espelho de transporte de <see cref="ParametrosOtimizacao"/>.</summary>
public sealed class ParametrosDto
{
    public double Volfrac { get; set; } = 0.3;
    public double Pitch { get; set; } = 1.0;
    public double Penal { get; set; } = 3.0;
    public double Rmin { get; set; } = 3.0;
    public int Maxloop { get; set; } = 2000;
    public double Tolx { get; set; } = 0.01;
    public int Nelx { get; set; } = 32;
    public int Nely { get; set; } = 16;
    public int Nelz { get; set; } = 16;

    public ParametrosOtimizacao ParaDominio() => new()
    {
        Volfrac = Volfrac,
        Pitch = Pitch,
        Penal = Penal,
        Rmin = Rmin,
        Maxloop = Maxloop,
        Tolx = Tolx,
        Nelx = Nelx,
        Nely = Nely,
        Nelz = Nelz
    };

    public static ParametrosDto De(ParametrosOtimizacao p) => new()
    {
        Volfrac = p.Volfrac,
        Pitch = p.Pitch,
        Penal = p.Penal,
        Rmin = p.Rmin,
        Maxloop = p.Maxloop,
        Tolx = p.Tolx,
        Nelx = p.Nelx,
        Nely = p.Nely,
        Nelz = p.Nelz
    };
}
