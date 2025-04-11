using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class IsYeriId_Foreignkey_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.AlterColumn<int>(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId",
                principalTable: "IS_YERI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.AlterColumn<int>(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId",
                principalTable: "IS_YERI",
                principalColumn: "Id");
        }
    }
}
