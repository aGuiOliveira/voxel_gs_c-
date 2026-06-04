namespace VoxelSpace.Api.Domain.Componentes;

/// <summary>
/// Suporte/bracket estrutural (ex.: o satellite_bracket que o engine otimiza).
/// Carrega cargas mecânicas entre subsistemas — quanto maior a carga, mais
/// crítico é manter rigidez mesmo após remover massa.
/// </summary>
public sealed class SuporteEstrutural : ComponenteEspacial
{
    /// <summary>Carga máxima suportada, em kN.</summary>
    public double CargaSuportadaKn { get; set; }

    public override decimal CalcularIndiceCriticidade()
    {
        // Cargas mais altas elevam a criticidade. Satura em 1.0 a 50 kN.
        var indice = (decimal)(CargaSuportadaKn / 50.0);
        return Math.Clamp(indice, 0m, 1m);
    }

    public override string ClassificacaoMissao() => "Estrutura primária";
}
