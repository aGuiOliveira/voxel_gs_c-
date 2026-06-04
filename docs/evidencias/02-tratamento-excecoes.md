# Evidências — Tratamento de exceções (resiliência)

A API nunca quebra: cada falha vira um ProblemDetails com status HTTP correto.

## 404 — GET componente inexistente
```json
{
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "Componente de id '9999' não foi encontrado.",
  "instance": "/api/componentes/9999"
}
```

## 409 — concluir uma execução que já está concluída (regra de negócio / transição inválida)
```json
{
  "title": "Conflito de regra de negócio",
  "status": 409,
  "detail": "Só uma execução em andamento pode ser concluída (status atual: Concluida).",
  "instance": "/api/execucoes/1/concluir"
}
```

## 400 — parâmetro inválido (volfrac fora de (0,1))
```json
{
  "title": "Parâmetro inválido",
  "status": 400,
  "detail": "volfrac deve estar em (0, 1).",
  "instance": "/api/execucoes"
}
```

## 404 — criar execução para componente inexistente
```json
{
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "Componente de id '424242' não foi encontrado.",
  "instance": "/api/execucoes"
}
```

## 409 — adicionar região a execução já finalizada
```json
{
  "title": "Conflito de regra de negócio",
  "status": 409,
  "detail": "Não é possível adicionar condições de contorno a uma execução finalizada (status: Concluida).",
  "instance": "/api/execucoes/1/regioes"
}
```
