using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class presicionEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropIndex(
                name: "IX_SAYAC_TANIMLARI_IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.RenameColumn(
                name: "Deger",
                table: "SAYAC_VERI",
                newName: "decimal(18,2)");

            migrationBuilder.RenameColumn(
                name: "Carpan",
                table: "ISLETME_SAYAC_DAGILIMI",
                newName: "decimal(7,6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "decimal(18,2)",
                table: "SAYAC_VERI",
                newName: "Deger");

            migrationBuilder.RenameColumn(
                name: "decimal(7,6)",
                table: "ISLETME_SAYAC_DAGILIMI",
                newName: "Carpan");

            migrationBuilder.AddColumn<int>(
                name: "IsletmeId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_TANIMLARI_IsletmeId",
                table: "SAYAC_TANIMLARI",
                column: "IsletmeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
                table: "SAYAC_TANIMLARI",
                column: "IsletmeId",
                principalTable: "ISLETME",
                principalColumn: "Id");
        }
    }
}
