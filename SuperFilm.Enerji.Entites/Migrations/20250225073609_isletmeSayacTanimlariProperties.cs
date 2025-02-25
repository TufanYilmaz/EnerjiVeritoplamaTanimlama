using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class isletmeSayacTanimlariProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SayacId",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_SayacId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "SayacId");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_TANIMLARI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "IsletmeId",
                principalTable: "ISLETME_TANIMLARI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_SAYAC_TANIMLARI_SayacId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "SayacId",
                principalTable: "SAYAC_TANIMLARI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_TANIMLARI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_SAYAC_TANIMLARI_SayacId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropIndex(
                name: "IX_ISLETME_SAYAC_DAGILIMI_SayacId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropColumn(
                name: "IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropColumn(
                name: "SayacId",
                table: "ISLETME_SAYAC_DAGILIMI");
        }
    }
}
