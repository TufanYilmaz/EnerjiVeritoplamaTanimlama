using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class sayacVeri_Kod_Eklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_VERI_SAYAC_TANIMLARI_SayacId",
                table: "SAYAC_VERI");

            migrationBuilder.DropIndex(
                name: "IX_SAYAC_VERI_SayacId",
                table: "SAYAC_VERI");

            migrationBuilder.AddColumn<string>(
                name: "Kod",
                table: "SAYAC_VERI",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OpcNodesId",
                table: "SAYAC_VERI",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kod",
                table: "SAYAC_VERI");

            migrationBuilder.DropColumn(
                name: "OpcNodesId",
                table: "SAYAC_VERI");

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_VERI_SayacId",
                table: "SAYAC_VERI",
                column: "SayacId");

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_VERI_SAYAC_TANIMLARI_SayacId",
                table: "SAYAC_VERI",
                column: "SayacId",
                principalTable: "SAYAC_TANIMLARI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
