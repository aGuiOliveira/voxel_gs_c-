namespace VoxelSpace.Api.Domain.Common;

/// <summary>
/// Traduz o resultado de engenharia (massa economizada) em valor de missão:
/// quanto custaria colocar aquela massa em órbita. É o "porquê espacial" da
/// otimização topológica — cada kg a menos é dinheiro economizado no lançamento.
/// </summary>
public static class CalculadoraLancamento
{
    /// <summary>
    /// Custo de referência por kg para órbita baixa (LEO), em dólares.
    /// Ordem de grandeza de um lançador comercial moderno.
    /// </summary>
    public const decimal CustoPorKgLeoUsd = 2_720m;

    /// <summary>
    /// Custo de lançamento (USD) economizado ao remover <paramref name="massaSalvaKg"/>
    /// de um componente. Massa negativa é tratada como zero (não há economia).
    /// </summary>
    public static decimal CustoEconomizadoUsd(decimal massaSalvaKg)
    {
        if (massaSalvaKg <= 0)
        {
            return 0m;
        }

        return Math.Round(massaSalvaKg * CustoPorKgLeoUsd, 2);
    }

    /// <summary>
    /// Massa economizada (kg) a partir das massas antes/depois da otimização.
    /// </summary>
    public static decimal MassaEconomizadaKg(decimal massaInicialKg, decimal massaFinalKg)
    {
        var economia = massaInicialKg - massaFinalKg;
        return economia > 0 ? economia : 0m;
    }
}
