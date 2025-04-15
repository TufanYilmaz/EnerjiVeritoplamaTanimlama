using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class IsYeri_columnNamesChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsletmeKodu",
                table: "ISYERI",
                newName: "Kodu");

            migrationBuilder.RenameColumn(
                name: "IsletmeAdi",
                table: "ISYERI",
                newName: "Adi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kodu",
                table: "ISYERI",
                newName: "IsletmeKodu");

            migrationBuilder.RenameColumn(
                name: "Adi",
                table: "ISYERI",
                newName: "IsletmeAdi");
        }
    }
}
