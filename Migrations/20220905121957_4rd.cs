using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animal_seller_api.Migrations
{
    public partial class _4rd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiableString");

            migrationBuilder.AddColumn<string>(
                name: "PostIds",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostIds",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "IdentifiableString",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentifiableString", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentifiableString_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiableString_UserId",
                table: "IdentifiableString",
                column: "UserId");
        }
    }
}
