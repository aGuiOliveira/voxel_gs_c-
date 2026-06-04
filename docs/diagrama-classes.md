# Diagrama de Classes — Domínio

Duas hierarquias de herança (componentes e regiões), o agregado de execução com
seu objeto de valor de parâmetros e a telemetria de iterações.

```mermaid
classDiagram
    class ComponenteEspacial {
        <<abstract>>
        +int Id
        +string Nome
        +MaterialAeroespacial Material
        +decimal MassaInicialKg
        +DateTime DataCadastro
        +CalcularIndiceCriticidade()* decimal
        +ClassificacaoMissao()* string
        +ResumoTecnico() string
    }
    class SuporteEstrutural {
        +double CargaSuportadaKn
    }
    class PainelSolar {
        +double AreaM2
        +double PotenciaW
    }
    class SuporteAntena {
        +double FrequenciaGhz
    }
    ComponenteEspacial <|-- SuporteEstrutural
    ComponenteEspacial <|-- PainelSolar
    ComponenteEspacial <|-- SuporteAntena

    class ExecucaoOtimizacao {
        +int Id
        +StatusExecucao Status
        +DateTime DataCriacao
        +DateTime? DataInicio
        +DateTime? DataFim
        +double? DuracaoSegundos
        +double? ReducaoPct
        +decimal? MassaFinalKg
        +Iniciar()
        +Concluir(ResultadoOtimizacao, MaterialAeroespacial)
        +RegistrarFalha(string)
        +Cancelar()
        +RegistrarIteracao(IteracaoOtimizacao)
        +MassaEconomizadaKg() decimal?
    }
    class ParametrosOtimizacao {
        <<owned>>
        +double Volfrac
        +double Penal
        +int Nelx
        +Validar()
    }
    class IteracaoOtimizacao {
        +int NumeroIteracao
        +double Compliance
        +double Change
        +DateTime RegistradoEm
    }

    class RegiaoFronteira {
        <<abstract>>
        +int Id
        +TipoCondicao TipoCondicao
        +double? Fx
        +CalcularVolumeMm3()* double
        +DescreverGeometria()* string
        +MagnitudeForca() double
    }
    class RegiaoEsferica {
        +double CentroX
        +double Raio
    }
    class RegiaoCaixa {
        +double MinX
        +double MaxX
    }
    class RegiaoFace {
        +FaceGrid Face
        +double EspessuraMm
    }
    RegiaoFronteira <|-- RegiaoEsferica
    RegiaoFronteira <|-- RegiaoCaixa
    RegiaoFronteira <|-- RegiaoFace

    ComponenteEspacial "1" --> "*" ExecucaoOtimizacao : Execucoes
    ExecucaoOtimizacao "1" *-- "1" ParametrosOtimizacao : Parametros
    ExecucaoOtimizacao "1" --> "*" IteracaoOtimizacao : Iteracoes
    ExecucaoOtimizacao "1" --> "*" RegiaoFronteira : Regioes
```

## Interfaces e injeção de dependência

```mermaid
classDiagram
    class IRepositorio~T~ {
        <<interface>>
        +ObterPorIdAsync(int) T
        +ListarAsync() IReadOnlyList~T~
        +AdicionarAsync(T)
        +SalvarAsync() int
    }
    class IComponenteRepository {
        <<interface>>
        +ObterComExecucoesAsync(int)
    }
    class IExecucaoRepository {
        <<interface>>
        +ListarFiltradoAsync(status, de, ate)
        +ListarConcluidasAsync()
    }
    class IExecucaoService {
        <<interface>>
        +CriarAsync() ExecucaoOtimizacao
        +IniciarAsync() ExecucaoOtimizacao
        +ConcluirAsync() ExecucaoOtimizacao
    }
    class IEstatisticaService {
        <<interface>>
        +ObterResumoAsync()
        +ObterRankingAsync(top)
    }

    IRepositorio <|-- IComponenteRepository
    IRepositorio <|-- IExecucaoRepository
    IComponenteRepository <|.. ComponenteRepository
    IExecucaoRepository <|.. ExecucaoRepository
    IExecucaoService <|.. ExecucaoService
    IEstatisticaService <|.. EstatisticaService
    ExecucaoService --> IExecucaoRepository
    ExecucaoService --> IComponenteRepository
    EstatisticaService --> IExecucaoRepository
```
