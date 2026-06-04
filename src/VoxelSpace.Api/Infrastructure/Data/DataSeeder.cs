using Microsoft.EntityFrameworkCore;
using VoxelSpace.Api.Domain.Componentes;
using VoxelSpace.Api.Domain.Enums;
using VoxelSpace.Api.Domain.Execucoes;
using VoxelSpace.Api.Domain.Regioes;

namespace VoxelSpace.Api.Infrastructure.Data;

/// <summary>
/// Popula o banco com dados de demonstração na primeira execução (se vazio):
/// componentes de tipos distintos e execuções em vários estados, incluindo uma
/// concluída com curva de convergência e condições de contorno. Garante que
/// todos os endpoints retornem dados úteis de imediato.
/// </summary>
public static class DataSeeder
{
    public static async Task SemearAsync(VoxelDbContext contexto, ILogger logger, CancellationToken ct = default)
    {
        if (await contexto.Componentes.AnyAsync(ct))
        {
            logger.LogInformation("Seed ignorado: banco já contém componentes.");
            return;
        }

        logger.LogInformation("Semeando dados de demonstração...");

        var bracket = new SuporteEstrutural
        {
            Nome = "Bracket de fixação do propulsor",
            Material = MaterialAeroespacial.Titanio6Al4V,
            MassaInicialKg = 2.214m, // ≈ 500 000 mm³ de titânio
            CargaSuportadaKn = 38.0
        };

        var painel = new PainelSolar
        {
            Nome = "Estrutura de painel solar",
            Material = MaterialAeroespacial.FibraCarbono,
            MassaInicialKg = 1.280m,
            AreaM2 = 12.5,
            PotenciaW = 1850.0
        };

        var antena = new SuporteAntena
        {
            Nome = "Suporte de antena banda Ka",
            Material = MaterialAeroespacial.Aluminio7075,
            MassaInicialKg = 0.703m,
            FrequenciaGhz = 27.0
        };

        var longarina = new SuporteEstrutural
        {
            Nome = "Longarina estrutural primária",
            Material = MaterialAeroespacial.Inconel718,
            MassaInicialKg = 6.140m,
            CargaSuportadaKn = 47.0
        };

        await contexto.Componentes.AddRangeAsync(new ComponenteEspacial[] { bracket, painel, antena, longarina }, ct);

        // Execução concluída, com regiões e curva de convergência.
        var concluida = CriarExecucao(bracket, volfrac: 0.30, penal: 3.0);
        AdicionarRegioesPadrao(concluida);
        concluida.Iniciar();
        PreencherConvergencia(concluida, complianceInicial: 1450.0, iteracoes: 24);
        concluida.Concluir(
            new ResultadoOtimizacao
            {
                VolumeDesignSpaceMm3 = 500_000,
                VolumeFinalMm3 = 178_500,
                EffectiveVolfrac = 0.357,
                Watertight = true,
                GridX = 100,
                GridY = 50,
                GridZ = 50,
                NComponentsDiscarded = 1,
                SolverElapsedS = 184.6
            },
            bracket.Material);

        // Execução concluída do painel.
        var concluidaPainel = CriarExecucao(painel, volfrac: 0.25, penal: 3.0);
        concluidaPainel.Iniciar();
        PreencherConvergencia(concluidaPainel, complianceInicial: 980.0, iteracoes: 18);
        concluidaPainel.Concluir(
            new ResultadoOtimizacao
            {
                VolumeDesignSpaceMm3 = 800_000,
                VolumeFinalMm3 = 232_000,
                EffectiveVolfrac = 0.29,
                Watertight = true,
                GridX = 160,
                GridY = 80,
                GridZ = 40,
                NComponentsDiscarded = 0,
                SolverElapsedS = 142.0
            },
            painel.Material);

        // Execução em andamento (com algumas iterações já registradas).
        var executando = CriarExecucao(antena, volfrac: 0.35, penal: 3.0);
        executando.Iniciar();
        PreencherConvergencia(executando, complianceInicial: 320.0, iteracoes: 7);

        // Execução que falhou (grid grande demais → estouro de memória no solver).
        var comErro = CriarExecucao(longarina, volfrac: 0.30, penal: 3.0);
        comErro.Parametros.Nelx = 400;
        comErro.Parametros.Nely = 200;
        comErro.Parametros.Nelz = 200;
        comErro.Iniciar();
        comErro.RegistrarFalha("Pardiso error -2: memória insuficiente para fatoração do grid (16M elementos).");

        await contexto.Execucoes.AddRangeAsync(
            new[] { concluida, concluidaPainel, executando, comErro }, ct);

        await contexto.SaveChangesAsync(ct);
        logger.LogInformation("Seed concluído: 4 componentes e 4 execuções.");
    }

    private static ExecucaoOtimizacao CriarExecucao(ComponenteEspacial componente, double volfrac, double penal)
        => new()
        {
            Componente = componente,
            Parametros = new ParametrosOtimizacao
            {
                Volfrac = volfrac,
                Penal = penal,
                Pitch = 1.0,
                Rmin = 3.0,
                Maxloop = 2000,
                Tolx = 0.01,
                Nelx = 100,
                Nely = 50,
                Nelz = 50
            }
        };

    private static void AdicionarRegioesPadrao(ExecucaoOtimizacao execucao)
    {
        // Apoio na face x_min (cantilever clássico).
        execucao.Regioes.Add(new RegiaoFace
        {
            TipoCondicao = TipoCondicao.Apoio,
            Face = FaceGrid.XMin,
            EspessuraMm = 2.0,
            AreaFaceMm2 = 2_500
        });

        // Força aplicada em uma esfera na ponta livre, apontando para -Z.
        execucao.Regioes.Add(new RegiaoEsferica
        {
            TipoCondicao = TipoCondicao.Forca,
            CentroX = 98.0,
            CentroY = 25.0,
            CentroZ = 25.0,
            Raio = 5.0,
            Fx = 0.0,
            Fy = 0.0,
            Fz = -1200.0
        });

        // Região keep-solid em caixa ao redor de um furo de fixação.
        execucao.Regioes.Add(new RegiaoCaixa
        {
            TipoCondicao = TipoCondicao.KeepSolid,
            MinX = 0.0,
            MinY = 10.0,
            MinZ = 10.0,
            MaxX = 8.0,
            MaxY = 40.0,
            MaxZ = 40.0
        });
    }

    /// <summary>
    /// Gera uma curva de convergência plausível: compliance caindo, 'change'
    /// diminuindo em direção à tolerância e fração de volume estabilizando.
    /// </summary>
    private static void PreencherConvergencia(ExecucaoOtimizacao execucao, double complianceInicial, int iteracoes)
    {
        var complianceAnterior = complianceInicial;

        for (var n = 1; n <= iteracoes; n++)
        {
            // Decaimento exponencial suave da compliance.
            var compliance = complianceInicial * (0.45 + 0.55 * Math.Exp(-0.18 * n));
            var delta = compliance - complianceAnterior;
            var change = 0.30 * Math.Exp(-0.16 * n);
            var volume = execucao.Parametros.Volfrac + 0.25 * Math.Exp(-0.20 * n);

            execucao.RegistrarIteracao(new IteracaoOtimizacao
            {
                NumeroIteracao = n,
                Compliance = Math.Round(compliance, 4),
                ComplianceDelta = Math.Round(delta, 4),
                VolumeFracao = Math.Round(volume, 4),
                Change = Math.Round(change, 4),
                TempoIteracaoS = Math.Round(6.5 + 0.5 * Math.Sin(n), 3)
            });

            complianceAnterior = compliance;
        }
    }
}
