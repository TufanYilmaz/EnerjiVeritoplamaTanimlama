using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class table_sayacTanimlari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsletmeAdi",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.RenameColumn(
                name: "IsletmeKodu",
                table: "ISLETME_TANIMLARI",
                newName: "SayacKodu");

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

            migrationBuilder.CreateTable(
                name: "SAYAC_TANIMLARI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsletmeKodu = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    IsletmeAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAYAC_TANIMLARI", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAYAC_TANIMLARI");

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
                name: "SayacKodu",
                table: "ISLETME_TANIMLARI",
                newName: "IsletmeKodu");

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
    }
}
