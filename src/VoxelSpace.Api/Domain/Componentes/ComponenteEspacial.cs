using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;

namespace VoxelSpace.Api.Domain.Componentes;

/// <summary>
/// Classe-base abstrata para qualquer equipamento estrutural espacial candidato
/// à otimização topológica (remoção de massa). É abstrata porque "componente
/// genérico" não existe na prática — sempre é um suporte, painel, antena, etc.,
/// cada qual com seu próprio critério de criticidade de missão.
///
/// Mapeada por herança Table-Per-Hierarchy (TPH): todas as subclasses vivem na
/// tabela COMPONENTES_ESPACIAIS com a coluna discriminadora TIPO.
/// </summary>
public abstract class ComponenteEspacial
{
    private string _nome = string.Empty;

    /// <summary>Chave primária (gerada pelo banco).</summary>
    public int Id { get; private set; }

    /// <summary>Nome do componente. Não pode ser vazio.</summary>
    public string Nome
    {
        get => _nome;
        set => _nome = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Nome do componente não pode ser vazio.", nameof(Nome))
            : value.Trim();
    }

    /// <summary>Material estrutural — define a densidade usada no cálculo de massa.</summary>
    public MaterialAeroespacial Material { get; set; }

    /// <summary>Massa do componente antes da otimização, em kg.</summary>
    public decimal MassaInicialKg { get; set; }

    /// <summary>Momento de cadastro (UTC).</summary>
    public DateTime DataCadastro { get; private set; } = DateTime.UtcNow;

    /// <summary>Execuções de otimização realizadas para este componente.</summary>
    public ICollection<ExecucaoOtimizacao> Execucoes { get; private set; } = new List<ExecucaoOtimizacao>();

    /// <summary>
    /// Índice de criticidade de massa/missão (0..1). Quanto maior, mais a missão
    /// se beneficia de reduzir a massa deste componente. Cada subclasse decide
    /// como calcular a partir de suas próprias características.
    /// </summary>
    public abstract decimal CalcularIndiceCriticidade();

    /// <summary>Classificação do papel do componente na missão.</summary>
    public abstract string ClassificacaoMissao();

    /// <summary>
    /// Resumo técnico legível, montado a partir dos membros abstratos
    /// (Template Method). Comum a todas as subclasses.
    /// </summary>
    public string ResumoTecnico()
    {
        var criticidade = CalcularIndiceCriticidade();
        return $"[{ClassificacaoMissao()}] {Nome} — {Material}, " +
               $"{MassaInicialKg:0.###} kg, criticidade {criticidade:P0}.";
    }
}
