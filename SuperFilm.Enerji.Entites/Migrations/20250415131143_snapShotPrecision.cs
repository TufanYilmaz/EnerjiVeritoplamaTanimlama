using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class snapShotPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "decimal(18,2)",
                table: "SAYAC_VERI",
                newName: "Deger");

            migrationBuilder.RenameColumn(
                name: "decimal(7,6)",
                table: "ISLETME_SAYAC_DAGILIMI",
                newName: "Carpan");

            migrationBuilder.AlterColumn<decimal>(
                name: "Carpan",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "decimal(7,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deger",
                table: "SAYAC_VERI",
                newName: "decimal(18,2)");

            migrationBuilder.RenameColumn(
                name: "Carpan",
                table: "ISLETME_SAYAC_DAGILIMI",
                newName: "decimal(7,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "decimal(7,6)",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,6)");
        }
    }
}
