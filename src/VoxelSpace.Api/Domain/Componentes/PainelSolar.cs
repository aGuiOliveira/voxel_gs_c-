namespace VoxelSpace.Api.Domain.Componentes;

/// <summary>
/// Estrutura de painel solar. Painéis grandes (mais área) dominam a massa
/// despendida em apêndices e têm forte impacto no orçamento de massa do satélite.
/// </summary>
public sealed class PainelSolar : ComponenteEspacial
{
    /// <summary>Área do painel, em m².</summary>
    public double AreaM2 { get; set; }

    /// <summary>Potência gerada, em W.</summary>
    public double PotenciaW { get; set; }

    public override decimal CalcularIndiceCriticidade()
    {
        // Área grande → mais estrutura → mais a ganhar com otimização. Satura a 20 m².
        var indice = (decimal)(AreaM2 / 20.0);
        return Math.Clamp(indice, 0m, 1m);
    }

    public override string ClassificacaoMissao() => "Geração de energia";
}
