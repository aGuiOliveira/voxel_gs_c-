using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Regioes;

namespace VoxelSpace.Api.DTOs.Regioes;

/// <summary>Representação de saída de uma condição de contorno.</summary>
public sealed record RegiaoResponseDto
{
    public int Id { get; init; }
    public int ExecucaoId { get; init; }
    public string Geometria { get; init; } = string.Empty;
    public TipoCondicao TipoCondicao { get; init; }
    public string Descricao { get; init; } = string.Empty;
    public double VolumeMm3 { get; init; }
    public double MagnitudeForcaN { get; init; }
    public double? Fx { get; init; }
    public double? Fy { get; init; }
    public double? Fz { get; init; }

    public static RegiaoResponseDto De(RegiaoFronteira r) => new()
    {
        Id = r.Id,
        ExecucaoId = r.ExecucaoId,
        Geometria = r.GetType().Name,
        TipoCondicao = r.TipoCondicao,
        Descricao = r.DescreverGeometria(),
        VolumeMm3 = r.CalcularVolumeMm3(),
        MagnitudeForcaN = r.MagnitudeForca(),
        Fx = r.Fx,
        Fy = r.Fy,
        Fz = r.Fz
    };
}
