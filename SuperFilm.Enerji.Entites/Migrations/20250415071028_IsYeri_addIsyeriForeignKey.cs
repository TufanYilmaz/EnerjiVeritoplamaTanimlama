using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class IsYeri_addIsyeriForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME");

            migrationBuilder.AlterColumn<int>(
                name: "IsyeriId",
                table: "ISLETME",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME",
                column: "IsyeriId",
                principalTable: "ISYERI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME");

            migrationBuilder.AlterColumn<int>(
                name: "IsyeriId",
                table: "ISLETME",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME",
                column: "IsyeriId",
                principalTable: "ISYERI",
                principalColumn: "Id");
        }
    }
}
