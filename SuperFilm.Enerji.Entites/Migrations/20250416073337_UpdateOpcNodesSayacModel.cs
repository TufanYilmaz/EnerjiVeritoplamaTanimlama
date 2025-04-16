using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperFilm.Enerji.Entites.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOpcNodesSayacModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OPC_NODES_SAYAC_DAGILIMI_SAYAC_TANIMLARI_SayacId",
            //    table: "OPC_NODES_SAYAC_DAGILIMI");


            //migrationBuilder.RenameTable(
            //    name: "OPC_NODES_SAYAC_DAGILIMI",
            //    newName: "OPC_NODES_ISLETME_DAGILIMI");

            
            //migrationBuilder.RenameColumn(
            //    name: "SayacId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI",
            //    newName: "IsletmeId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_OPC_NODES_SAYAC_DAGILIMI_SayacId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI",
            //    newName: "IX_OPC_NODES_ISLETME_DAGILIMI_IsletmeId");

            
            //migrationBuilder.AddForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_DAGILIMI_ISLETME_IsletmeId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI",
            //    column: "IsletmeId",
            //    principalTable: "ISLETME",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OPC_NODES_ISLETME_DAGILIMI_ISLETME_IsletmeId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI");

            
            //migrationBuilder.RenameIndex(
            //    name: "IX_OPC_NODES_ISLETME_DAGILIMI_IsletmeId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI",
            //    newName: "IX_OPC_NODES_SAYAC_DAGILIMI_SayacId");

            //migrationBuilder.RenameColumn(
            //    name: "IsletmeId",
            //    table: "OPC_NODES_ISLETME_DAGILIMI",
            //    newName: "SayacId");    

           
            //migrationBuilder.RenameTable(
            //    name: "OPC_NODES_ISLETME_DAGILIMI",
            //    newName: "OPC_NODES_SAYAC_DAGILIMI");

            
            //migrationBuilder.AddForeignKey(
            //    name: "FK_OPC_NODES_SAYAC_DAGILIMI_SAYAC_TANIMLARI_SayacId",
            //    table: "OPC_NODES_SAYAC_DAGILIMI",
            //    column: "SayacId",
            //    principalTable: "SAYAC_TANIMLARI",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
