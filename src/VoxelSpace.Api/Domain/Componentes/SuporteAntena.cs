namespace VoxelSpace.Api.Domain.Componentes;

/// <summary>
/// Suporte de antena de comunicação. Antenas de alta frequência exigem maior
/// estabilidade dimensional (a estrutura não pode vibrar/deformar), o que torna
/// a remoção de massa mais delicada — e portanto mais crítica.
/// </summary>
public sealed class SuporteAntena : ComponenteEspacial
{
    /// <summary>Frequência de operação da antena, em GHz.</summary>
    public double FrequenciaGhz { get; set; }

    public override decimal CalcularIndiceCriticidade()
    {
        // Frequências mais altas exigem mais rigidez dimensional. Satura a 40 GHz (banda Ka).
        var indice = (decimal)(FrequenciaGhz / 40.0);
        return Math.Clamp(indice, 0m, 1m);
    }

    public override string ClassificacaoMissao() => "Comunicação";
}
