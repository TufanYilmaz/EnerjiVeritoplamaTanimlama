using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class removeProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_ISYERI_IsletmeTanimlariId",
                table: "ISLETME");

            migrationBuilder.RenameColumn(
                name: "IsletmeTanimlariId",
                table: "ISLETME",
                newName: "IsyeriId");

            migrationBuilder.RenameIndex(
                name: "IX_ISLETME_IsletmeTanimlariId",
                table: "ISLETME",
                newName: "IX_ISLETME_IsyeriId");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME",
                column: "IsyeriId",
                principalTable: "ISYERI",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_ISYERI_IsyeriId",
                table: "ISLETME");

            migrationBuilder.RenameColumn(
                name: "IsyeriId",
                table: "ISLETME",
                newName: "IsletmeTanimlariId");

            migrationBuilder.RenameIndex(
                name: "IX_ISLETME_IsyeriId",
                table: "ISLETME",
                newName: "IX_ISLETME_IsletmeTanimlariId");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_ISYERI_IsletmeTanimlariId",
                table: "ISLETME",
                column: "IsletmeTanimlariId",
                principalTable: "ISYERI",
                principalColumn: "Id");
        }
    }
}
