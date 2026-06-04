# Evidências de execução — VoxelSpace API

Capturado em 2026-06-04T18:15:58Z | provider=SQLite (fallback local, sem Docker) | porta 5080
Mesmos endpoints e respostas valem para Oracle XE — só muda DatabaseProvider.

## 1) GET /healthz
```json
{
  "ok": true,
  "servico": "VoxelSpace API"
}
```

## 2) GET /api/componentes  (dados de seed — herança polimórfica)
```json
[
  {
    "id": 1,
    "tipo": "PainelSolar",
    "nome": "Estrutura de painel solar",
    "material": "FibraCarbono",
    "materialNome": "FibraCarbono",
    "massaInicialKg": 1.28,
    "dataCadastroUtc": "2026-06-04T18:14:03.7801797",
    "dataCadastroLocal": "2026-06-04T15:14:03.7801797",
    "indiceCriticidade": 0.625,
    "classificacaoMissao": "Geração de energia",
    "resumoTecnico": "[Geração de energia] Estrutura de painel solar — FibraCarbono, 1,28 kg, criticidade 63%.",
    "qtdExecucoes": 0,
    "cargaSuportadaKn": null,
    "areaM2": 12.5,
    "potenciaW": 1850,
    "frequenciaGhz": null
  },
  {
    "id": 2,
    "tipo": "SuporteAntena",
    "nome": "Suporte de antena banda Ka",
    "material": "Aluminio7075",
    "materialNome": "Aluminio7075",
    "massaInicialKg": 0.703,
    "dataCadastroUtc": "2026-06-04T18:14:03.7802495",
    "dataCadastroLocal": "2026-06-04T15:14:03.7802495",
    "indiceCriticidade": 0.675,
    "classificacaoMissao": "Comunicação",
    "resumoTecnico": "[Comunicação] Suporte de antena banda Ka — Aluminio7075, 0,703 kg, criticidade 68%.",
    "qtdExecucoes": 0,
    "cargaSuportadaKn": null,
    "areaM2": null,
    "potenciaW": null,
    "frequenciaGhz": 27
  },
  {
    "id": 3,
    "tipo": "SuporteEstrutural",
    "nome": "Bracket de fixação do propulsor",
    "material": "Titanio6Al4V",
    "materialNome": "Titanio6Al4V",
    "massaInicialKg": 2.214,
    "dataCadastroUtc": "2026-06-04T18:14:03.7799604",
    "dataCadastroLocal": "2026-06-04T15:14:03.7799604",
    "indiceCriticidade": 0.76,
    "classificacaoMissao": "Estrutura primária",
    "resumoTecnico": "[Estrutura primária] Bracket de fixação do propulsor — Titanio6Al4V, 2,214 kg, criticidade 76%.",
    "qtdExecucoes": 0,
    "cargaSuportadaKn": 38,
    "areaM2": null,
    "potenciaW": null,
    "frequenciaGhz": null
  },
  {
    "id": 4,
    "tipo": "SuporteEstrutural",
    "nome": "Longarina estrutural primária",
    "material": "Inconel718",
    "materialNome": "Inconel718",
    "massaInicialKg": 6.14,
    "dataCadastroUtc": "2026-06-04T18:14:03.7802713",
    "dataCadastroLocal": "2026-06-04T15:14:03.7802713",
    "indiceCriticidade": 0.94,
    "classificacaoMissao": "Estrutura primária",
    "resumoTecnico": "[Estrutura primária] Longarina estrutural primária — Inconel718, 6,14 kg, criticidade 94%.",
    "qtdExecucoes": 0,
    "cargaSuportadaKn": 47,
    "areaM2": null,
    "potenciaW": null,
    "frequenciaGhz": null
  },
  {
    "id": 5,
    "tipo": "SuporteEstrutural",
    "nome": "Suporte de bateria",
    "material": "Aluminio7075",
    "materialNome": "Aluminio7075",
    "massaInicialKg": 1.5,
    "dataCadastroUtc": "2026-06-04T18:14:35.0949743",
    "dataCadastroLocal": "2026-06-04T15:14:35.0949743",
    "indiceCriticidade": 0.44,
    "classificacaoMissao": "Estrutura primária",
    "resumoTecnico": "[Estrutura primária] Suporte de bateria — Aluminio7075, 1,5 kg, criticidade 44%.",
    "qtdExecucoes": 0,
    "cargaSuportadaKn": 22,
    "areaM2": null,
    "potenciaW": null,
    "frequenciaGhz": null
  }
]
```

## 3) GET /api/execucoes/1  (execução concluída, com métricas e custo de lançamento)
```json
{
  "id": 1,
  "componenteId": 3,
  "componenteNome": "Bracket de fixação do propulsor",
  "status": "Concluida",
  "statusNome": "Concluida",
  "dataCriacaoUtc": "2026-06-04T18:14:03.8564045",
  "dataInicioUtc": "2026-06-04T18:14:03.8575374",
  "dataFimUtc": "2026-06-04T18:14:03.8612277",
  "dataCriacaoLocal": "2026-06-04T15:14:03.8564045",
  "duracaoSegundos": 0.0036903,
  "parametros": {
    "volfrac": 0.3,
    "pitch": 1,
    "penal": 3,
    "rmin": 3,
    "maxloop": 2000,
    "tolx": 0.01,
    "nelx": 100,
    "nely": 50,
    "nelz": 50
  },
  "volumeDesignSpaceMm3": 500000,
  "volumeFinalMm3": 178500,
  "reducaoPct": 64.3,
  "effectiveVolfrac": 0.357,
  "watertight": true,
  "gridX": 100,
  "gridY": 50,
  "gridZ": 50,
  "nComponentsDiscarded": 1,
  "solverElapsedS": 184.6,
  "massaFinalKg": 0.790755,
  "massaEconomizadaKg": 1.423245,
  "custoEconomizadoUsd": 3871.23,
  "mensagemErro": null,
  "qtdIteracoes": 0,
  "qtdRegioes": 3
}
```

## 4) GET /api/execucoes/1/iteracoes  (curva de convergência — série temporal)
```json
[
  {
    "id": 1,
    "execucaoId": 1,
    "numeroIteracao": 1,
    "compliance": 1318.628,
    "complianceDelta": -131.372,
    "volumeFracao": 0.5047,
    "change": 0.2556,
    "tempoIteracaoS": 6.921,
    "registradoEmUtc": "2026-06-04T18:14:03.8578693",
    "registradoEmLocal": "2026-06-04T15:14:03.8578693"
  },
  {
    "id": 2,
    "execucaoId": 1,
    "numeroIteracao": 2,
    "compliance": 1208.8969,
    "complianceDelta": -109.7311,
    "volumeFracao": 0.4676,
    "change": 0.2178,
    "tempoIteracaoS": 6.955,
    "registradoEmUtc": "2026-06-04T18:14:03.8581706",
    "registradoEmLocal": "2026-06-04T15:14:03.8581706"
  },
  {
    "id": 3,
    "execucaoId": 1,
    "numeroIteracao": 3,
    "compliance": 1117.2417,
    "complianceDelta": -91.6551,
    "volumeFracao": 0.4372,
    "change": 0.1856,
    "tempoIteracaoS": 6.571,
    "registradoEmUtc": "2026-06-04T18:14:03.8581714",
    "registradoEmLocal": "2026-06-04T15:14:03.8581714"
  },
  {
    "id": 4,
    "execucaoId": 1,
    "numeroIteracao": 4,
    "compliance": 1040.6849,
    "complianceDelta": -76.5568,
    "volumeFracao": 0.4123,
    "change": 0.1582,
    "tempoIteracaoS": 6.122,
    "registradoEmUtc": "2026-06-04T18:14:03.8581716",
    "registradoEmLocal": "2026-06-04T15:14:03.8581716"
  },
  {
    "id": 5,
    "execucaoId": 1,
    "numeroIteracao": 5,
    "compliance": 976.7393,
    "complianceDelta": -63.9456,
    "volumeFracao": 0.392,
    "change": 0.1348,
    "tempoIteracaoS": 6.021,
    "registradoEmUtc": "2026-06-04T18:14:03.8581717",
    "registradoEmLocal": "2026-06-04T15:14:03.8581717"
  },
  {
    "id": 6,
    "execucaoId": 1,
    "numeroIteracao": 6,
    "compliance": 923.3274,
    "complianceDelta": -53.4119,
    "volumeFracao": 0.3753,
    "change": 0.1149,
    "tempoIteracaoS": 6.36,
    "registradoEmUtc": "2026-06-04T18:14:03.8581724",
    "registradoEmLocal": "2026-06-04T15:14:03.8581724"
  },
  {
    "id": 7,
    "execucaoId": 1,
    "numeroIteracao": 7,
    "compliance": 878.7141,
    "complianceDelta": -44.6133,
    "volumeFracao": 0.3616,
    "change": 0.0979,
    "tempoIteracaoS": 6.828,
    "registradoEmUtc": "2026-06-04T18:14:03.8581737",
    "registradoEmLocal": "2026-06-04T15:14:03.8581737"
  },
  {
    "id": 8,
    "execucaoId": 1,
    "numeroIteracao": 8,
    "compliance": 841.4499,
    "complianceDelta": -37.2642,
    "volumeFracao": 0.3505,
    "change": 0.0834,
    "tempoIteracaoS": 6.995,
    "registradoEmUtc": "2026-06-04T18:14:03.8581738",
    "registradoEmLocal": "2026-06-04T15:14:03.8581738"
  },
  {
    "id": 9,
    "execucaoId": 1,
    "numeroIteracao": 9,
    "compliance": 810.3242,
    "complianceDelta": -31.1257,
    "volumeFracao": 0.3413,
    "change": 0.0711,
    "tempoIteracaoS": 6.706,
    "registradoEmUtc": "2026-06-04T18:14:03.8581739",
    "registradoEmLocal": "2026-06-04T15:14:03.8581739"
  },
  {
    "id": 10,
    "execucaoId": 1,
    "numeroIteracao": 10,
    "compliance": 784.3259,
    "complianceDelta": -25.9983,
    "volumeFracao": 0.3338,
    "change": 0.0606,
    "tempoIteracaoS": 6.228,
    "registradoEmUtc": "2026-06-04T18:14:03.8581742",
    "registradoEmLocal": "2026-06-04T15:14:03.8581742"
  },
  {
    "id": 11,
    "execucaoId": 1,
    "numeroIteracao": 11,
    "compliance": 762.6102,
    "complianceDelta": -21.7156,
    "volumeFracao": 0.3277,
    "change": 0.0516,
    "tempoIteracaoS": 6,
    "registradoEmUtc": "2026-06-04T18:14:03.8581743",
    "registradoEmLocal": "2026-06-04T15:14:03.8581743"
  },
  {
    "id": 12,
    "execucaoId": 1,
    "numeroIteracao": 12,
    "compliance": 744.4718,
    "complianceDelta": -18.1384,
    "volumeFracao": 0.3227,
    "change": 0.044,
    "tempoIteracaoS": 6.232,
    "registradoEmUtc": "2026-06-04T18:14:03.8581744",
    "registradoEmLocal": "2026-06-04T15:14:03.8581744"
  },
  {
    "id": 13,
    "execucaoId": 1,
    "numeroIteracao": 13,
    "compliance": 729.3213,
    "complianceDelta": -15.1505,
    "volumeFracao": 0.3186,
    "change": 0.0375,
    "tempoIteracaoS": 6.71,
    "registradoEmUtc": "2026-06-04T18:14:03.8581746",
    "registradoEmLocal": "2026-06-04T15:14:03.8581746"
  },
  {
    "id": 14,
    "execucaoId": 1,
    "numeroIteracao": 14,
    "compliance": 716.6665,
    "complianceDelta": -12.6548,
    "volumeFracao": 0.3152,
    "change": 0.0319,
    "tempoIteracaoS": 6.995,
    "registradoEmUtc": "2026-06-04T18:14:03.8581747",
    "registradoEmLocal": "2026-06-04T15:14:03.8581747"
  },
  {
    "id": 15,
    "execucaoId": 1,
    "numeroIteracao": 15,
    "compliance": 706.0964,
    "complianceDelta": -10.5701,
    "volumeFracao": 0.3124,
    "change": 0.0272,
    "tempoIteracaoS": 6.825,
    "registradoEmUtc": "2026-06-04T18:14:03.8581748",
    "registradoEmLocal": "2026-06-04T15:14:03.8581748"
  },
  {
    "id": 16,
    "execucaoId": 1,
    "numeroIteracao": 16,
    "compliance": 697.2675,
    "complianceDelta": -8.8289,
    "volumeFracao": 0.3102,
    "change": 0.0232,
    "tempoIteracaoS": 6.356,
    "registradoEmUtc": "2026-06-04T18:14:03.858175",
    "registradoEmLocal": "2026-06-04T15:14:03.858175"
  },
  {
    "id": 17,
    "execucaoId": 1,
    "numeroIteracao": 17,
    "compliance": 689.8929,
    "complianceDelta": -7.3745,
    "volumeFracao": 0.3083,
    "change": 0.0198,
    "tempoIteracaoS": 6.019,
    "registradoEmUtc": "2026-06-04T18:14:03.8581751",
    "registradoEmLocal": "2026-06-04T15:14:03.8581751"
  },
  {
    "id": 18,
    "execucaoId": 1,
    "numeroIteracao": 18,
    "compliance": 683.7332,
    "complianceDelta": -6.1597,
    "volumeFracao": 0.3068,
    "change": 0.0168,
    "tempoIteracaoS": 6.125,
    "registradoEmUtc": "2026-06-04T18:14:03.8581753",
    "registradoEmLocal": "2026-06-04T15:14:03.8581753"
  },
  {
    "id": 19,
    "execucaoId": 1,
    "numeroIteracao": 19,
    "compliance": 678.5882,
    "complianceDelta": -5.145,
    "volumeFracao": 0.3056,
    "change": 0.0144,
    "tempoIteracaoS": 6.575,
    "registradoEmUtc": "2026-06-04T18:14:03.8581755",
    "registradoEmLocal": "2026-06-04T15:14:03.8581755"
  },
  {
    "id": 20,
    "execucaoId": 1,
    "numeroIteracao": 20,
    "compliance": 674.2907,
    "complianceDelta": -4.2975,
    "volumeFracao": 0.3046,
    "change": 0.0122,
    "tempoIteracaoS": 6.956,
    "registradoEmUtc": "2026-06-04T18:14:03.8581756",
    "registradoEmLocal": "2026-06-04T15:14:03.8581756"
  },
  {
    "id": 21,
    "execucaoId": 1,
    "numeroIteracao": 21,
    "compliance": 670.7011,
    "complianceDelta": -3.5896,
    "volumeFracao": 0.3037,
    "change": 0.0104,
    "tempoIteracaoS": 6.918,
    "registradoEmUtc": "2026-06-04T18:14:03.8581757",
    "registradoEmLocal": "2026-06-04T15:14:03.8581757"
  },
  {
    "id": 22,
    "execucaoId": 1,
    "numeroIteracao": 22,
    "compliance": 667.7028,
    "complianceDelta": -2.9983,
    "volumeFracao": 0.3031,
    "change": 0.0089,
    "tempoIteracaoS": 6.496,
    "registradoEmUtc": "2026-06-04T18:14:03.8581759",
    "registradoEmLocal": "2026-06-04T15:14:03.8581759"
  },
  {
    "id": 23,
    "execucaoId": 1,
    "numeroIteracao": 23,
    "compliance": 665.1985,
    "complianceDelta": -2.5044,
    "volumeFracao": 0.3025,
    "change": 0.0076,
    "tempoIteracaoS": 6.077,
    "registradoEmUtc": "2026-06-04T18:14:03.858176",
    "registradoEmLocal": "2026-06-04T15:14:03.858176"
  },
  {
    "id": 24,
    "execucaoId": 1,
    "numeroIteracao": 24,
    "compliance": 663.1067,
    "complianceDelta": -2.0918,
    "volumeFracao": 0.3021,
    "change": 0.0064,
    "tempoIteracaoS": 6.047,
    "registradoEmUtc": "2026-06-04T18:14:03.8581762",
    "registradoEmLocal": "2026-06-04T15:14:03.8581762"
  }
]
```

## 5) GET /api/execucoes/1/regioes  (condições de contorno — apoio/força/keep-solid)
```json
[
  {
    "id": 1,
    "execucaoId": 1,
    "geometria": "RegiaoCaixa",
    "tipoCondicao": "KeepSolid",
    "descricao": "Caixa [0,10,10]→[8,40,40]",
    "volumeMm3": 7200,
    "magnitudeForcaN": 0,
    "fx": null,
    "fy": null,
    "fz": null
  },
  {
    "id": 2,
    "execucaoId": 1,
    "geometria": "RegiaoEsferica",
    "tipoCondicao": "Forca",
    "descricao": "Esfera r=5 em (98, 25, 25)",
    "volumeMm3": 523.5987755982989,
    "magnitudeForcaN": 1200,
    "fx": 0,
    "fy": 0,
    "fz": -1200
  },
  {
    "id": 3,
    "execucaoId": 1,
    "geometria": "RegiaoFace",
    "tipoCondicao": "Apoio",
    "descricao": "Face XMin (espessura 2 mm)",
    "volumeMm3": 5000,
    "magnitudeForcaN": 0,
    "fx": null,
    "fy": null,
    "fz": null
  }
]
```

## 6) GET /api/estatisticas/resumo  (analytics de missão)
```json
{
  "totalExecucoes": 5,
  "totalConcluidas": 3,
  "taxaWatertightPct": 100,
  "reducaoMediaPct": 68.43,
  "reducaoMaximaPct": 71,
  "tempoMedioSolverS": 140.6,
  "massaEconomizadaTotalKg": 3.4948,
  "custoEconomizadoTotalUsd": 9505.99
}
```

## 7) GET /api/estatisticas/por-material
```json
[
  {
    "material": "Titanio6Al4V",
    "materialNome": "Titanio6Al4V",
    "qtdExecucoes": 1,
    "reducaoMediaPct": 64.3,
    "reducaoMaximaPct": 64.3,
    "massaEconomizadaKg": 1.4232,
    "custoEconomizadoUsd": 3871.23
  },
  {
    "material": "Aluminio7075",
    "materialNome": "Aluminio7075",
    "qtdExecucoes": 1,
    "reducaoMediaPct": 70,
    "reducaoMaximaPct": 70,
    "massaEconomizadaKg": 1.1628,
    "custoEconomizadoUsd": 3162.82
  },
  {
    "material": "FibraCarbono",
    "materialNome": "FibraCarbono",
    "qtdExecucoes": 1,
    "reducaoMediaPct": 71,
    "reducaoMaximaPct": 71,
    "massaEconomizadaKg": 0.9088,
    "custoEconomizadoUsd": 2471.94
  }
]
```

## 8) GET /api/estatisticas/ranking?top=5
```json
[
  {
    "posicao": 1,
    "execucaoId": 2,
    "componenteNome": "Estrutura de painel solar",
    "material": "FibraCarbono",
    "reducaoPct": 71,
    "massaEconomizadaKg": 0.9088,
    "custoEconomizadoUsd": 2471.94
  },
  {
    "posicao": 2,
    "execucaoId": 5,
    "componenteNome": "Suporte de bateria",
    "material": "Aluminio7075",
    "reducaoPct": 70,
    "massaEconomizadaKg": 1.1628,
    "custoEconomizadoUsd": 3162.82
  },
  {
    "posicao": 3,
    "execucaoId": 1,
    "componenteNome": "Bracket de fixação do propulsor",
    "material": "Titanio6Al4V",
    "reducaoPct": 64.3,
    "massaEconomizadaKg": 1.4232,
    "custoEconomizadoUsd": 3871.23
  }
]
```

## 9) GET /api/execucoes?status=Concluida&de=2026-01-01&ate=2026-12-31  (filtro por status + janela temporal)
```json
[
  {
    "id": 5,
    "componenteId": 5,
    "componenteNome": "Suporte de bateria",
    "status": "Concluida",
    "statusNome": "Concluida",
    "dataCriacaoUtc": "2026-06-04T18:14:35.827682",
    "dataInicioUtc": "2026-06-04T18:14:36.6818137",
    "dataFimUtc": "2026-06-04T18:14:37.1723126",
    "dataCriacaoLocal": "2026-06-04T15:14:35.827682",
    "duracaoSegundos": 0.4904989,
    "parametros": {
      "volfrac": 0.3,
      "pitch": 1,
      "penal": 3,
      "rmin": 3,
      "maxloop": 2000,
      "tolx": 0.01,
      "nelx": 80,
      "nely": 40,
      "nelz": 40
    },
    "volumeDesignSpaceMm3": 400000,
    "volumeFinalMm3": 120000,
    "reducaoPct": 70,
    "effectiveVolfrac": 0.3,
    "watertight": true,
    "gridX": 80,
    "gridY": 40,
    "gridZ": 40,
    "nComponentsDiscarded": 0,
    "solverElapsedS": 95.2,
    "massaFinalKg": 0.3372,
    "massaEconomizadaKg": 1.1628,
    "custoEconomizadoUsd": 3162.82,
    "mensagemErro": null,
    "qtdIteracoes": 0,
    "qtdRegioes": 0
  },
  {
    "id": 2,
    "componenteId": 1,
    "componenteNome": "Estrutura de painel solar",
    "status": "Concluida",
    "statusNome": "Concluida",
    "dataCriacaoUtc": "2026-06-04T18:14:03.8612574",
    "dataInicioUtc": "2026-06-04T18:14:03.8612583",
    "dataFimUtc": "2026-06-04T18:14:03.8612756",
    "dataCriacaoLocal": "2026-06-04T15:14:03.8612574",
    "duracaoSegundos": 1.73e-05,
    "parametros": {
      "volfrac": 0.25,
      "pitch": 1,
      "penal": 3,
      "rmin": 3,
      "maxloop": 2000,
      "tolx": 0.01,
      "nelx": 100,
      "nely": 50,
      "nelz": 50
    },
    "volumeDesignSpaceMm3": 800000,
    "volumeFinalMm3": 232000,
    "reducaoPct": 71,
    "effectiveVolfrac": 0.29,
    "watertight": true,
    "gridX": 160,
    "gridY": 80,
    "gridZ": 40,
    "nComponentsDiscarded": 0,
    "solverElapsedS": 142,
    "massaFinalKg": 0.3712,
    "massaEconomizadaKg": 0.9088,
    "custoEconomizadoUsd": 2471.94,
    "mensagemErro": null,
    "qtdIteracoes": 0,
    "qtdRegioes": 0
  },
  {
    "id": 1,
    "componenteId": 3,
    "componenteNome": "Bracket de fixação do propulsor",
    "status": "Concluida",
    "statusNome": "Concluida",
    "dataCriacaoUtc": "2026-06-04T18:14:03.8564045",
    "dataInicioUtc": "2026-06-04T18:14:03.8575374",
    "dataFimUtc": "2026-06-04T18:14:03.8612277",
    "dataCriacaoLocal": "2026-06-04T15:14:03.8564045",
    "duracaoSegundos": 0.0036903,
    "parametros": {
      "volfrac": 0.3,
      "pitch": 1,
      "penal": 3,
      "rmin": 3,
      "maxloop": 2000,
      "tolx": 0.01,
      "nelx": 100,
      "nely": 50,
      "nelz": 50
    },
    "volumeDesignSpaceMm3": 500000,
    "volumeFinalMm3": 178500,
    "reducaoPct": 64.3,
    "effectiveVolfrac": 0.357,
    "watertight": true,
    "gridX": 100,
    "gridY": 50,
    "gridZ": 50,
    "nComponentsDiscarded": 1,
    "solverElapsedS": 184.6,
    "massaFinalKg": 0.790755,
    "massaEconomizadaKg": 1.423245,
    "custoEconomizadoUsd": 3871.23,
    "mensagemErro": null,
    "qtdIteracoes": 0,
    "qtdRegioes": 0
  }
]
```
