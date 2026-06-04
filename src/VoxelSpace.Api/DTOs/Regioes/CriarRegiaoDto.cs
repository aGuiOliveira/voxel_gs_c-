using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Regioes;
using VoxelSpace.Api.Exceptions;

namespace VoxelSpace.Api.DTOs.Regioes;

/// <summary>
/// Dados para criar uma condição de contorno. <see cref="Geometria"/> escolhe a
/// subclasse concreta; os campos da geometria correspondente são lidos.
/// </summary>
public sealed class CriarRegiaoDto
{
    public TipoGeometria Geometria { get; set; }
    public TipoCondicao TipoCondicao { get; set; }

    // Vetor de força (opcional; usado quando TipoCondicao = Forca).
    public double? Fx { get; set; }
    public double? Fy { get; set; }
    public double? Fz { get; set; }

    // Esfera
    public double? CentroX { get; set; }
    public double? CentroY { get; set; }
    public double? CentroZ { get; set; }
    public double? Raio { get; set; }

    // Caixa
    public double? MinX { get; set; }
    public double? MinY { get; set; }
    public double? MinZ { get; set; }
    public double? MaxX { get; set; }
    public double? MaxY { get; set; }
    public double? MaxZ { get; set; }

    // Face
    public FaceGrid? Face { get; set; }
    public double? EspessuraMm { get; set; }
    public double? AreaFaceMm2 { get; set; }

    /// <summary>
    /// Constrói a entidade de domínio da geometria correta. Lança
    /// <see cref="ParametroInvalidoException"/> se faltarem campos obrigatórios.
    /// </summary>
    public RegiaoFronteira ParaDominio()
    {
        RegiaoFronteira regiao = Geometria switch
        {
            TipoGeometria.Esfera => new RegiaoEsferica
            {
                CentroX = Exigir(CentroX, nameof(CentroX)),
                CentroY = Exigir(CentroY, nameof(CentroY)),
                CentroZ = Exigir(CentroZ, nameof(CentroZ)),
                Raio = Exigir(Raio, nameof(Raio))
            },
            TipoGeometria.Caixa => new RegiaoCaixa
            {
                MinX = Exigir(MinX, nameof(MinX)),
                MinY = Exigir(MinY, nameof(MinY)),
                MinZ = Exigir(MinZ, nameof(MinZ)),
                MaxX = Exigir(MaxX, nameof(MaxX)),
                MaxY = Exigir(MaxY, nameof(MaxY)),
                MaxZ = Exigir(MaxZ, nameof(MaxZ))
            },
            TipoGeometria.Face => new RegiaoFace
            {
                Face = Face ?? throw new ParametroInvalidoException("Campo 'Face' é obrigatório para geometria Face."),
                EspessuraMm = EspessuraMm ?? 1.0,
                AreaFaceMm2 = Exigir(AreaFaceMm2, nameof(AreaFaceMm2))
            },
            _ => throw new ParametroInvalidoException($"Geometria desconhecida: {Geometria}.")
        };

        regiao.TipoCondicao = TipoCondicao;

        if (TipoCondicao == TipoCondicao.Forca)
        {
            regiao.Fx = Exigir(Fx, nameof(Fx));
            regiao.Fy = Exigir(Fy, nameof(Fy));
            regiao.Fz = Exigir(Fz, nameof(Fz));
        }
        else
        {
            // Mesmo sem ser força, aceita vetor se vier (não obrigatório).
            regiao.Fx = Fx;
            regiao.Fy = Fy;
            regiao.Fz = Fz;
        }

        return regiao;
    }

    private static double Exigir(double? valor, string campo)
        => valor ?? throw new ParametroInvalidoException($"Campo '{campo}' é obrigatório para esta geometria/condição.");
}
