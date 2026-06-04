using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.Domain.Regioes;

/// <summary>
/// Condição de contorno aplicada a uma execução: apoio, força ou região
/// keep-solid. A geometria (esfera/caixa/face) varia por subclasse, então a
/// base é abstrata e cada forma sabe calcular seu próprio volume.
///
/// Mapeada por herança Table-Per-Hierarchy (TPH): tabela REGIOES_FRONTEIRA com a
/// coluna discriminadora GEOMETRIA. Reflete os discriminated unions
/// (sphere/box/face) do engine.
/// </summary>
public abstract class RegiaoFronteira
{
    public int Id { get; private set; }

    /// <summary>FK para a execução.</summary>
    public int ExecucaoId { get; set; }

    public ExecucaoOtimizacao? Execucao { get; set; }

    /// <summary>Papel da região (apoio, força ou keep-solid).</summary>
    public TipoCondicao TipoCondicao { get; set; }

    // Vetor de força (Fx, Fy, Fz). Preenchido apenas quando TipoCondicao == Forca.
    public double? Fx { get; set; }
    public double? Fy { get; set; }
    public double? Fz { get; set; }

    /// <summary>Volume aproximado ocupado pela região (mm³). Implementado por geometria.</summary>
    public abstract double CalcularVolumeMm3();

    /// <summary>Descrição curta da geometria, para listagens/logs.</summary>
    public abstract string DescreverGeometria();

    /// <summary>true se a região carrega um vetor de força válido.</summary>
    public bool TemVetorForca() => Fx.HasValue && Fy.HasValue && Fz.HasValue;

    /// <summary>Magnitude do vetor de força (N), ou 0 se não houver vetor.</summary>
    public double MagnitudeForca()
    {
        if (!TemVetorForca())
        {
            return 0.0;
        }

        return Math.Sqrt(Fx!.Value * Fx.Value + Fy!.Value * Fy.Value + Fz!.Value * Fz.Value);
    }
}
