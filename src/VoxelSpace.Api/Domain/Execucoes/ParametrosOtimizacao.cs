using VoxelSpace.Api.Exceptions;

namespace VoxelSpace.Api.Domain.Execucoes;

/// <summary>
/// Conjunto de parâmetros do solver SIMP. Modelado como objeto de valor
/// (owned type do EF Core): não tem identidade própria, vive embutido na
/// <see cref="ExecucaoOtimizacao"/>. Espelha os campos de OptimizeRequest do engine.
/// </summary>
public sealed class ParametrosOtimizacao
{
    /// <summary>Fração de volume alvo (0..1).</summary>
    public double Volfrac { get; set; } = 0.3;

    /// <summary>Tamanho do voxel (mm).</summary>
    public double Pitch { get; set; } = 1.0;

    /// <summary>Penalização SIMP.</summary>
    public double Penal { get; set; } = 3.0;

    /// <summary>Raio do filtro de sensibilidade.</summary>
    public double Rmin { get; set; } = 3.0;

    /// <summary>Número máximo de iterações.</summary>
    public int Maxloop { get; set; } = 2000;

    /// <summary>Tolerância de convergência em 'change'.</summary>
    public double Tolx { get; set; } = 0.01;

    public int Nelx { get; set; } = 32;
    public int Nely { get; set; } = 16;
    public int Nelz { get; set; } = 16;

    /// <summary>
    /// Valida as faixas dos parâmetros. Lança <see cref="ParametroInvalidoException"/>
    /// na primeira violação encontrada — a API responde 400 sem quebrar.
    /// </summary>
    public void Validar()
    {
        if (Volfrac is <= 0 or >= 1)
        {
            throw new ParametroInvalidoException("volfrac deve estar em (0, 1).");
        }

        if (Pitch <= 0)
        {
            throw new ParametroInvalidoException("pitch deve ser positivo.");
        }

        if (Penal <= 0)
        {
            throw new ParametroInvalidoException("penal deve ser positivo.");
        }

        if (Rmin <= 0)
        {
            throw new ParametroInvalidoException("rmin deve ser positivo.");
        }

        if (Maxloop <= 0)
        {
            throw new ParametroInvalidoException("maxloop deve ser positivo.");
        }

        if (Nelx <= 0 || Nely <= 0 || Nelz <= 0)
        {
            throw new ParametroInvalidoException("dimensões do grid (nelx/nely/nelz) devem ser positivas.");
        }
    }

    /// <summary>Quantidade total de elementos do grid (proxy de custo de memória).</summary>
    public long TotalElementos() => (long)Nelx * Nely * Nelz;
}
