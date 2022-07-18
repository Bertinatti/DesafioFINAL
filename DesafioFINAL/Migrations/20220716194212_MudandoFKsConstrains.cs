using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioFINAL.Migrations
{
    public partial class MudandoFKsConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos",
                column: "IdProduto",
                principalTable: "Produtos",
                principalColumn: "IdProduto",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProdutos_Produtos_IdProduto",
                table: "LogProdutos",
                column: "IdProduto",
                principalTable: "Produtos",
                principalColumn: "IdProduto");
        }
    }
}
