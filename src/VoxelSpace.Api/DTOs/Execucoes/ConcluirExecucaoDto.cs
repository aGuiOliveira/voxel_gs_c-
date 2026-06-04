using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.DTOs.Execucoes;

/// <summary>Resultados finais informados ao concluir uma execução.</summary>
public sealed class ConcluirExecucaoDto
{
    public double VolumeDesignSpaceMm3 { get; set; }
    public double VolumeFinalMm3 { get; set; }
    public double EffectiveVolfrac { get; set; }
    public bool Watertight { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int GridZ { get; set; }
    public int NComponentsDiscarded { get; set; }
    public double SolverElapsedS { get; set; }

    public ResultadoOtimizacao ParaDominio() => new()
    {
        VolumeDesignSpaceMm3 = VolumeDesignSpaceMm3,
        VolumeFinalMm3 = VolumeFinalMm3,
        EffectiveVolfrac = EffectiveVolfrac,
        Watertight = Watertight,
        GridX = GridX,
        GridY = GridY,
        GridZ = GridZ,
        NComponentsDiscarded = NComponentsDiscarded,
        SolverElapsedS = SolverElapsedS
    };
}
