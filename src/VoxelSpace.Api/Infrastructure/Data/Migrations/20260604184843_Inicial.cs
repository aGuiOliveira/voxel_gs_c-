using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VoxelSpace.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPONENTES_ESPACIAIS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    Material = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MassaInicialKg = table.Column<decimal>(type: "DECIMAL(12,4)", precision: 12, scale: 4, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    AreaM2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PotenciaW = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    FrequenciaGhz = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CargaSuportadaKn = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMPONENTES_ESPACIAIS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EXECUCOES_OTIMIZACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ComponenteId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARAM_VOLFRAC = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PARAM_PITCH = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PARAM_PENAL = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PARAM_RMIN = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PARAM_MAXLOOP = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARAM_TOLX = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    PARAM_NELX = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARAM_NELY = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARAM_NELZ = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DataFim = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MensagemErro = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    VolumeDesignSpaceMm3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    VolumeFinalMm3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    ReducaoPct = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    EffectiveVolfrac = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    Watertight = table.Column<int>(type: "NUMBER(1)", nullable: true),
                    GridX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GridY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GridZ = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NComponentsDiscarded = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SolverElapsedS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MassaFinalKg = table.Column<decimal>(type: "DECIMAL(12,4)", precision: 12, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXECUCOES_OTIMIZACAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EXECUCOES_OTIMIZACAO_COMPONENTES_ESPACIAIS_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "COMPONENTES_ESPACIAIS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ITERACOES_OTIMIZACAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ExecucaoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NumeroIteracao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Compliance = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    ComplianceDelta = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    VolumeFracao = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Change = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TempoIteracaoS = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    RegistradoEm = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITERACOES_OTIMIZACAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ITERACOES_OTIMIZACAO_EXECUCOES_OTIMIZACAO_ExecucaoId",
                        column: x => x.ExecucaoId,
                        principalTable: "EXECUCOES_OTIMIZACAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REGIOES_FRONTEIRA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ExecucaoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoCondicao = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Fx = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    Fy = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    Fz = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    GEOMETRIA = table.Column<string>(type: "NVARCHAR2(21)", maxLength: 21, nullable: false),
                    MinX = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MinY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MinZ = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MaxX = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MaxY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MaxZ = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CentroX = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CentroY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CentroZ = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    Raio = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    Face = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EspessuraMm = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    AreaFaceMm2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REGIOES_FRONTEIRA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_REGIOES_FRONTEIRA_EXECUCOES_OTIMIZACAO_ExecucaoId",
                        column: x => x.ExecucaoId,
                        principalTable: "EXECUCOES_OTIMIZACAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EXECUCOES_OTIMIZACAO_ComponenteId",
                table: "EXECUCOES_OTIMIZACAO",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_EXECUCOES_OTIMIZACAO_DataCriacao",
                table: "EXECUCOES_OTIMIZACAO",
                column: "DataCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_EXECUCOES_OTIMIZACAO_Status",
                table: "EXECUCOES_OTIMIZACAO",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ITERACOES_OTIMIZACAO_ExecucaoId_NumeroIteracao",
                table: "ITERACOES_OTIMIZACAO",
                columns: new[] { "ExecucaoId", "NumeroIteracao" });

            migrationBuilder.CreateIndex(
                name: "IX_REGIOES_FRONTEIRA_ExecucaoId",
                table: "REGIOES_FRONTEIRA",
                column: "ExecucaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ITERACOES_OTIMIZACAO");

            migrationBuilder.DropTable(
                name: "REGIOES_FRONTEIRA");

            migrationBuilder.DropTable(
                name: "EXECUCOES_OTIMIZACAO");

            migrationBuilder.DropTable(
                name: "COMPONENTES_ESPACIAIS");
        }
    }
}
