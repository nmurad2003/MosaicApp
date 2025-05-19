using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MosaicApp.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Architectors_PositionGetVM_PositionId",
                table: "Architectors");

            migrationBuilder.DropTable(
                name: "PositionGetVM");

            migrationBuilder.AddForeignKey(
                name: "FK_Architectors_Positions_PositionId",
                table: "Architectors",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Architectors_Positions_PositionId",
                table: "Architectors");

            migrationBuilder.CreateTable(
                name: "PositionGetVM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionGetVM", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Architectors_PositionGetVM_PositionId",
                table: "Architectors",
                column: "PositionId",
                principalTable: "PositionGetVM",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
