using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class IsletmeTanimlariId_Null_Yapıldı : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.AlterColumn<int>(
                name: "IsletmeTanimlariId",
                table: "IS_YERI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "IsletmeTanimlariId",
                table: "IS_YERI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI",
                column: "IsletmeTanimlariId",
                principalTable: "ISLETME_TANIMLARI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
