using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendHorus.Migrations
{
    /// <inheritdoc />
    public partial class AddRecolectorRutaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecolectorRutas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MacrorutaId = table.Column<int>(type: "int", nullable: false),
                    MicrorutaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecolectorRutas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecolectorRutas_Macrorutas_MacrorutaId",
                        column: x => x.MacrorutaId,
                        principalTable: "Macrorutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecolectorRutas_Microrutas_MicrorutaId",
                        column: x => x.MicrorutaId,
                        principalTable: "Microrutas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecolectorRutas_MacrorutaId",
                table: "RecolectorRutas",
                column: "MacrorutaId");

            migrationBuilder.CreateIndex(
                name: "IX_RecolectorRutas_MicrorutaId",
                table: "RecolectorRutas",
                column: "MicrorutaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecolectorRutas");
        }
    }
}
