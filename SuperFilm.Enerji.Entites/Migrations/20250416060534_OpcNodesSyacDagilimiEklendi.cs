﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class OpcNodesSyacDagilimiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OPC_NODES_SAYAC_DAGİLİMİ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpcNodesId = table.Column<int>(type: "int", nullable: false),
                    SayacId = table.Column<int>(type: "int", nullable: false),
                    Islem = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Carpan = table.Column<decimal>(type: "decimal(7,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPC_NODES_SAYAC_DAGİLİMİ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPC_NODES_SAYAC_DAGİLİMİ_OPC_NODES_OpcNodesId",
                        column: x => x.OpcNodesId,
                        principalTable: "OPC_NODES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPC_NODES_SAYAC_DAGİLİMİ_SAYAC_TANIMLARI_SayacId",
                        column: x => x.SayacId,
                        principalTable: "SAYAC_TANIMLARI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OPC_NODES_SAYAC_DAGİLİMİ_OpcNodesId",
                table: "OPC_NODES_SAYAC_DAGİLİMİ",
                column: "OpcNodesId");

            migrationBuilder.CreateIndex(
                name: "IX_OPC_NODES_SAYAC_DAGİLİMİ_SayacId",
                table: "OPC_NODES_SAYAC_DAGİLİMİ",
                column: "SayacId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OPC_NODES_SAYAC_DAGİLİMİ");
        }
    }
}
