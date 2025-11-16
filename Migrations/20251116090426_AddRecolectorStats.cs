using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendHorus.Migrations
{
    /// <inheritdoc />
    public partial class AddRecolectorStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecolectorStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecolectorId = table.Column<int>(type: "int", nullable: false),
                    BolsasTotales = table.Column<int>(type: "int", nullable: false),
                    CoberturaPromedio = table.Column<double>(type: "float", nullable: false),
                    CantidadRecorridos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecolectorStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecolectorStats_Recolectores_RecolectorId",
                        column: x => x.RecolectorId,
                        principalTable: "Recolectores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecolectorStats_RecolectorId",
                table: "RecolectorStats",
                column: "RecolectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecolectorStats");
        }
    }
}
