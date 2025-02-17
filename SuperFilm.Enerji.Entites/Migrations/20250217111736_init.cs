using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ISLETME_TANIMLARI",
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
                    table.PrimaryKey("PK_ISLETME_TANIMLARI", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ISLETME_TANIMLARI");
        }
    }
}
