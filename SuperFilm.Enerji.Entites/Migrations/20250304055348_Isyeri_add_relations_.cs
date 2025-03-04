using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class Isyeri_add_relations_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsYeriId",
                table: "ISLETME_TANIMLARI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Carpan",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Islem",
                table: "ISLETME_SAYAC_DAGILIMI",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "IS_YERI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Ad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IS_YERI", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_TANIMLARI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId",
                principalTable: "IS_YERI",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_TANIMLARI_IS_YERI_IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropTable(
                name: "IS_YERI");

            migrationBuilder.DropIndex(
                name: "IX_SAYAC_TANIMLARI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropIndex(
                name: "IX_ISLETME_TANIMLARI_IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsYeriId",
                table: "ISLETME_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "Carpan",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropColumn(
                name: "Islem",
                table: "ISLETME_SAYAC_DAGILIMI");
        }
    }
}
