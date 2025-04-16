using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class OpsNodesSayacDagilimi_Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
            //    table: "SAYAC_TANIMLARI");

            //migrationBuilder.DropColumn(
            //    name: "IsletmeId",
            //    table: "SAYAC_TANIMLARI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "IsletmeId",
            //    table: "SAYAC_TANIMLARI",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
            //    table: "SAYAC_TANIMLARI",
            //    column: "IsletmeId",
            //    principalTable: "ISLETME",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
