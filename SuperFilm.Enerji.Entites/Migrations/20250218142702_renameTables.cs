using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class renameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsletmeAdi",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "SayacAciklama",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "SayacTanimi",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "SayacYeri",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.RenameColumn(
                name: "IsletmeKodu",
                table: "SAYAC_TANIMLARI",
                newName: "SayacKodu");

            migrationBuilder.RenameColumn(
                name: "SayacKodu",
                table: "ISLETME_TANIMLARI",
                newName: "IsletmeKodu");

            migrationBuilder.AddColumn<string>(
                name: "SayacAciklama",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SayacTanimi",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SayacYeri",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "ISLETME_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsletmeAdi",
                table: "ISLETME_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SayacAciklama",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "SayacTanimi",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "SayacYeri",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsletmeAdi",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.RenameColumn(
                name: "SayacKodu",
                table: "SAYAC_TANIMLARI",
                newName: "IsletmeKodu");

            migrationBuilder.RenameColumn(
                name: "IsletmeKodu",
                table: "ISLETME_TANIMLARI",
                newName: "SayacKodu");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsletmeAdi",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SayacAciklama",
                table: "ISLETME_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SayacTanimi",
                table: "ISLETME_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SayacYeri",
                table: "ISLETME_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
