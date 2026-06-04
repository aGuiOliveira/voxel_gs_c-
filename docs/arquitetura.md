# Arquitetura & Fluxo

## Camadas

```mermaid
flowchart TB
    Cliente["Cliente HTTP / Swagger UI"]
    subgraph API["VoxelSpace.Api (ASP.NET Core)"]
        MW["ExceptionHandlingMiddleware<br/>(ProblemDetails 400/404/409/500)"]
        Ctrl["Controllers<br/>Componentes · Execucoes · Estatisticas"]
        Svc["Services<br/>ExecucaoService · EstatisticaService"]
        Repo["Repositories (EF Core)<br/>Componente · Execucao"]
        Dom["Domain<br/>entidades + regras + POO"]
    end
    DB[("Banco<br/>Oracle XE | SQLite")]

    Cliente --> MW --> Ctrl
    Ctrl -->|DTOs| Svc
    Ctrl -->|leituras| Repo
    Svc --> Repo
    Svc --> Dom
    Repo --> Dom
    Repo --> DB
```

A camada de domínio não depende de nada acima dela. Controllers e serviços
dependem de **interfaces** (`IRepositorio<T>`, `IExecucaoService`,
`IEstatisticaService`), resolvidas por injeção de dependência no `Program.cs`.

## Ciclo de vida de uma execução (máquina de estados)

```mermaid
stateDiagram-v2
    [*] --> Enfileirada : POST /api/execucoes
    Enfileirada --> Executando : iniciar()
    Enfileirada --> Cancelada : cancelar()
    Executando --> Concluida : concluir(resultado)
    Executando --> Erro : registrarFalha(msg)
    Executando --> Cancelada : cancelar()
    Concluida --> [*]
    Erro --> [*]
    Cancelada --> [*]
```

Transições inválidas (ex.: concluir algo já concluído) lançam
`RegraNegocioException` → HTTP 409. A telemetria de iterações só é aceita no
estado `Executando`.

## Fluxo "criar e concluir uma execução"

```mermaid
sequenceDiagram
    participant C as Cliente
    participant E as ExecucoesController
    participant S as ExecucaoService
    participant R as IExecucaoRepository
    participant DB as Banco

    C->>E: POST /api/execucoes {componenteId, parametros}
    E->>S: CriarAsync(...)
    S->>S: parametros.Validar()
    S->>R: ObterPorIdAsync(componenteId)
    R-->>S: ComponenteEspacial (ou 404)
    S->>R: AdicionarAsync(execucao) + SalvarAsync()
    R->>DB: INSERT
    E-->>C: 201 Created (Enfileirada)

    C->>E: POST /api/execucoes/{id}/concluir {resultado}
    E->>S: ConcluirAsync(id, resultado)
    S->>S: execucao.Concluir() -> calcula reducaoPct + massaFinalKg
    S->>R: SalvarAsync()
    E-->>C: 200 OK (Concluida + métricas)
```
