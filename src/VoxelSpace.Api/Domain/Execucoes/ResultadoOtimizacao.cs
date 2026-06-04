namespace VoxelSpace.Api.Domain.Execucoes;

/// <summary>
/// Pacote de resultados finais entregue ao concluir uma execução. Espelha o dict
/// retornado por run_optimization no engine (volumes, grid, watertight, tempos).
/// </summary>
public sealed record ResultadoOtimizacao
{
    public double VolumeDesignSpaceMm3 { get; init; }
    public double VolumeFinalMm3 { get; init; }
    public double EffectiveVolfrac { get; init; }
    public bool Watertight { get; init; }
    public int GridX { get; init; }
    public int GridY { get; init; }
    public int GridZ { get; init; }
    public int NComponentsDiscarded { get; init; }
    public double SolverElapsedS { get; init; }
}
