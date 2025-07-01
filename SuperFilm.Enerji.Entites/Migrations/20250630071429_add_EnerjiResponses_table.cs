using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class add_EnerjiResponses_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnerjiResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<string>(type: "varchar(10)", nullable: true),
                    StartTime = table.Column<string>(type: "varchar(10)", nullable: true),
                    EndDate = table.Column<string>(type: "varchar(10)", nullable: true),
                    EndTime = table.Column<string>(type: "varchar(10)", nullable: true),
                    UretimYeri = table.Column<string>(type: "varchar(10)", nullable: true),
                    DDeger = table.Column<double>(type: "float", nullable: false),
                    EDeger = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnerjiResponses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnerjiResponses");
        }
    }
}
