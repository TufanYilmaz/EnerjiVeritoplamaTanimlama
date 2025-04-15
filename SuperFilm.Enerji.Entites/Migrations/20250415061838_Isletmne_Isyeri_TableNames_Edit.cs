using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class Isletmne_Isyeri_TableNames_Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_TANIMLARI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropTable(
                name: "ISLETME_TANIMLARI");

            migrationBuilder.DropIndex(
                name: "IX_SAYAC_TANIMLARI_IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IS_YERI",
                table: "IS_YERI");

            migrationBuilder.DropIndex(
                name: "IX_IS_YERI_IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.DropColumn(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "IS_YERI");

            migrationBuilder.DropColumn(
                name: "IsletmeTanimlariId",
                table: "IS_YERI");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "IS_YERI");

            migrationBuilder.RenameTable(
                name: "IS_YERI",
                newName: "ISYERI");

            migrationBuilder.RenameColumn(
                name: "Kod",
                table: "ISYERI",
                newName: "IsletmeKodu");

            migrationBuilder.AlterColumn<string>(
                name: "SayacYeri",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "IsletmeId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsletmeAdi",
                table: "ISYERI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ISYERI",
                table: "ISYERI",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ISLETME",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Ad = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Tip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsletmeTanimlariId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISLETME", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ISLETME_ISYERI_IsletmeTanimlariId",
                        column: x => x.IsletmeTanimlariId,
                        principalTable: "ISYERI",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_TANIMLARI_IsletmeId",
                table: "SAYAC_TANIMLARI",
                column: "IsletmeId");

            migrationBuilder.CreateIndex(
                name: "IX_ISLETME_IsletmeTanimlariId",
                table: "ISLETME",
                column: "IsletmeTanimlariId");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "IsletmeId",
                principalTable: "ISLETME",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
                table: "SAYAC_TANIMLARI",
                column: "IsletmeId",
                principalTable: "ISLETME",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI");

            migrationBuilder.DropForeignKey(
                name: "FK_SAYAC_TANIMLARI_ISLETME_IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropTable(
                name: "ISLETME");

            migrationBuilder.DropIndex(
                name: "IX_SAYAC_TANIMLARI_IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ISYERI",
                table: "ISYERI");

            migrationBuilder.DropColumn(
                name: "IsletmeId",
                table: "SAYAC_TANIMLARI");

            migrationBuilder.DropColumn(
                name: "IsletmeAdi",
                table: "ISYERI");

            migrationBuilder.RenameTable(
                name: "ISYERI",
                newName: "IS_YERI");

            migrationBuilder.RenameColumn(
                name: "IsletmeKodu",
                table: "IS_YERI",
                newName: "Kod");

            migrationBuilder.AlterColumn<string>(
                name: "SayacYeri",
                table: "SAYAC_TANIMLARI",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IsYeriId",
                table: "SAYAC_TANIMLARI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "IS_YERI",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IsletmeTanimlariId",
                table: "IS_YERI",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tip",
                table: "IS_YERI",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IS_YERI",
                table: "IS_YERI",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ISLETME_TANIMLARI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aciklama = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsletmeAdi = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsletmeKodu = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISLETME_TANIMLARI", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAYAC_TANIMLARI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId");

            migrationBuilder.CreateIndex(
                name: "IX_IS_YERI_IsletmeTanimlariId",
                table: "IS_YERI",
                column: "IsletmeTanimlariId");

            migrationBuilder.AddForeignKey(
                name: "FK_IS_YERI_ISLETME_TANIMLARI_IsletmeTanimlariId",
                table: "IS_YERI",
                column: "IsletmeTanimlariId",
                principalTable: "ISLETME_TANIMLARI",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ISLETME_SAYAC_DAGILIMI_ISLETME_TANIMLARI_IsletmeId",
                table: "ISLETME_SAYAC_DAGILIMI",
                column: "IsletmeId",
                principalTable: "ISLETME_TANIMLARI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SAYAC_TANIMLARI_IS_YERI_IsYeriId",
                table: "SAYAC_TANIMLARI",
                column: "IsYeriId",
                principalTable: "IS_YERI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
