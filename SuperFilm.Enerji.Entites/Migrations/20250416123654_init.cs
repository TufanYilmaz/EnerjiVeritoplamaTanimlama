using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ISYERI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kodu = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Adi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISYERI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OPC_NODES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    NodeId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    NodeNameSpace = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPC_NODES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAYAC_TANIMLARI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SayacKodu = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    SayacTanimi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    SayacAciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    SayacYeri = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAYAC_TANIMLARI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ISLETME",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Ad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsyeriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISLETME", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ISLETME_ISYERI_IsyeriId",
                        column: x => x.IsyeriId,
                        principalTable: "ISYERI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SAYAC_VERI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yil = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    Ay = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    Gun = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    Zaman = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    NormalizeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deger = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SayacId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAYAC_VERI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAYAC_VERI_SAYAC_TANIMLARI_SayacId",
                        column: x => x.SayacId,
                        principalTable: "SAYAC_TANIMLARI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ISLETME_SAYAC_DAGILIMI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsletmeId = table.Column<int>(type: "int", nullable: false),
                    SayacId = table.Column<int>(type: "int", nullable: false),
                    Islem = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Carpan = table.Column<decimal>(type: "decimal(7,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISLETME_SAYAC_DAGILIMI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_IsletmeId",
                        column: x => x.IsletmeId,
                        principalTable: "ISLETME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ISLETME_SAYAC_DAGILIMI_SAYAC_TANIMLARI_SayacId",
                        column: x => x.SayacId,
                        principalTable: "SAYAC_TANIMLARI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OPC_NODES_ISLETME_DAGILIMI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpcNodesId = table.Column<int>(type: "int", nullable: false),
                    IsletmeId = table.Column<int>(type: "int", nullable: false),
                    Islem = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Carpan = table.Column<decimal>(type: "decimal(7,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPC_NODES_ISLETME_DAGILIMI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPC_NODES_ISLETME_DAGILIMI_ISLETME_IsletmeId",
                        column: x => x.IsletmeId,
                        principalTable: "ISLETME",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPC_NODES_ISLETME_DAGILIMI_OPC_NODES_OpcNodesId",
                        column: x => x.OpcNodesId,
                        principalTable: "OPC_NODES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_IsyeriId",
                table: "ISLETME",
                column: "IsyeriId");

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_SayacId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "SayacId");

            migrationBuilder.CreateIndex(
                name: "IX_OPC_NODES_ISLETME_DAGILIMI_IsletmeId",
                table: "OPC_NODES_ISLETME_DAGILIMI",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_OPC_NODES_ISLETME_DAGILIMI_OpcNodesId",
                table: "OPC_NODES_ISLETME_DAGILIMI",
                column: "OpcNodesId");

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_VERI_SayacId",
                table: "SAYAC_VERI",
                column: "SayacId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropTable(
                name: "OPC_NODES_ISLETME_DAGILIMI");

            migrationBuilder.DropTable(
                name: "SAYAC_VERI");

            migrationBuilder.DropTable(
                name: "ISLETME");

            migrationBuilder.DropTable(
                name: "OPC_NODES");

            migrationBuilder.DropTable(
                name: "SAYAC_TANIMLARI");

            migrationBuilder.DropTable(
                name: "ISYERI");
        }
    }
}
