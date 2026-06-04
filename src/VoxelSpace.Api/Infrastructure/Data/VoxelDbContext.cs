using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.Domain.Regioes;

namespace VoxelSpace.Api.Infrastructure.Data;

/// <summary>
/// Contexto EF Core do domínio de otimização topológica. Configura as duas
/// hierarquias TPH (componentes e regiões), o owned type de parâmetros, precisão
/// de decimais e índices. Funciona tanto sobre Oracle quanto sobre SQLite — só o
/// provider muda (configurado no <c>Program.cs</c>).
/// </summary>
public sealed class VoxelDbContext : DbContext
{
    public VoxelDbContext(DbContextOptions<VoxelDbContext> options) : base(options)
    {
    }

    public DbSet<ComponenteEspacial> Componentes => Set<ComponenteEspacial>();
    public DbSet<ExecucaoOtimizacao> Execucoes => Set<ExecucaoOtimizacao>();
    public DbSet<IteracaoOtimizacao> Iteracoes => Set<IteracaoOtimizacao>();
    public DbSet<RegiaoFronteira> Regioes => Set<RegiaoFronteira>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarComponentes(modelBuilder);
        ConfigurarExecucoes(modelBuilder);
        ConfigurarIteracoes(modelBuilder);
        ConfigurarRegioes(modelBuilder);
    }

    private static void ConfigurarComponentes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComponenteEspacial>(e =>
        {
            e.ToTable("COMPONENTES_ESPACIAIS");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(120);
            e.Property(c => c.Material).HasConversion<int>();
            e.Property(c => c.MassaInicialKg).HasPrecision(12, 4);
            e.Property(c => c.DataCadastro).IsRequired();

            // TPH: uma tabela, discriminador TIPO.
            e.HasDiscriminator<string>("TIPO")
                .HasValue<SuporteEstrutural>("SUPORTE_ESTRUTURAL")
                .HasValue<PainelSolar>("PAINEL_SOLAR")
                .HasValue<SuporteAntena>("SUPORTE_ANTENA");
        });

        modelBuilder.Entity<SuporteEstrutural>().Property(c => c.CargaSuportadaKn);
        modelBuilder.Entity<PainelSolar>(e =>
        {
            e.Property(c => c.AreaM2);
            e.Property(c => c.PotenciaW);
        });
        modelBuilder.Entity<SuporteAntena>().Property(c => c.FrequenciaGhz);
    }

    private static void ConfigurarExecucoes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExecucaoOtimizacao>(e =>
        {
            e.ToTable("EXECUCOES_OTIMIZACAO");
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).HasConversion<int>();
            e.Property(x => x.DataCriacao).IsRequired();
            e.Property(x => x.MensagemErro).HasMaxLength(2000);
            e.Property(x => x.MassaFinalKg).HasPrecision(12, 4);

            // Oracle XE 21c não tem o tipo SQL BOOLEAN (só o 23c+). O provider
            // 9.x geraria BOOLEAN e quebraria com ORA-00902, então mapeamos o
            // bool? para NUMBER(1) (0/1/null) explicitamente.
            e.Property(x => x.Watertight)
                .HasConversion<int>()
                .HasColumnType("NUMBER(1)");

            // Índices para os filtros expostos pela API.
            e.HasIndex(x => x.Status);
            e.HasIndex(x => x.DataCriacao);

            e.HasOne(x => x.Componente)
                .WithMany(c => c.Execucoes)
                .HasForeignKey(x => x.ComponenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Parâmetros como owned type (colunas embutidas na própria tabela).
            e.OwnsOne(x => x.Parametros, p =>
            {
                p.Property(v => v.Volfrac).HasColumnName("PARAM_VOLFRAC");
                p.Property(v => v.Pitch).HasColumnName("PARAM_PITCH");
                p.Property(v => v.Penal).HasColumnName("PARAM_PENAL");
                p.Property(v => v.Rmin).HasColumnName("PARAM_RMIN");
                p.Property(v => v.Maxloop).HasColumnName("PARAM_MAXLOOP");
                p.Property(v => v.Tolx).HasColumnName("PARAM_TOLX");
                p.Property(v => v.Nelx).HasColumnName("PARAM_NELX");
                p.Property(v => v.Nely).HasColumnName("PARAM_NELY");
                p.Property(v => v.Nelz).HasColumnName("PARAM_NELZ");
            });
        });
    }

    private static void ConfigurarIteracoes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IteracaoOtimizacao>(e =>
        {
            e.ToTable("ITERACOES_OTIMIZACAO");
            e.HasKey(i => i.Id);
            e.Property(i => i.RegistradoEm).IsRequired();

            e.HasOne(i => i.Execucao)
                .WithMany(x => x.Iteracoes)
                .HasForeignKey(i => i.ExecucaoId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(i => new { i.ExecucaoId, i.NumeroIteracao });
        });
    }

    private static void ConfigurarRegioes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegiaoFronteira>(e =>
        {
            e.ToTable("REGIOES_FRONTEIRA");
            e.HasKey(r => r.Id);
            e.Property(r => r.TipoCondicao).HasConversion<int>();

            e.HasOne(r => r.Execucao)
                .WithMany(x => x.Regioes)
                .HasForeignKey(r => r.ExecucaoId)
                .OnDelete(DeleteBehavior.Cascade);

            // TPH: discriminador GEOMETRIA.
            e.HasDiscriminator<string>("GEOMETRIA")
                .HasValue<RegiaoEsferica>("ESFERA")
                .HasValue<RegiaoCaixa>("CAIXA")
                .HasValue<RegiaoFace>("FACE");
        });

        modelBuilder.Entity<RegiaoFace>().Property(r => r.Face).HasConversion<int>();
    }
}
