using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class isyeri_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_TANIMLARI_IS_YERI_IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropIndex(
                name: "IX_ISLETME_TANIMLARI_IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.AddColumn<int>(
                name: "IsletmeTanimlariId",
                table: "IS_YERI",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IS_YERI_IsletmeTanimlariId",
                table: "IS_YERI",
                column: "IsletmeTanimlariId");

            migrationBuilder.AddForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI",
                column: "IsletmeTanimlariId",
                principalTable: "ISLETME_TANIMLARI",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.DropIndex(
                name: "IX_IS_YERI_IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.DropColumn(
                name: "IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.AddColumn<int>(
                name: "IsYeriId",
                table: "ISLETME_TANIMLARI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_TANIMLARI_IsYeriId",
                table: "ISLETME_TANIMLARI",
                column: "IsYeriId");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_TANIMLARI_IS_YERI_IsYeriId",
                table: "ISLETME_TANIMLARI",
                column: "IsYeriId",
                principalTable: "IS_YERI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
