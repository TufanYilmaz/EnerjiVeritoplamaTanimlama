using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class sayacveri_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAYAC_VERI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yil = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    Ay = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_VERI_SayacId",
                table: "SAYAC_VERI",
                column: "SayacId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAYAC_VERI");
        }
    }
}
