using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_banThucPhamSach.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateUpdateChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    AdminId = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MessageC__3214EC072642574E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageChat_Admin",
                        column: x => x.AdminId,
                        principalTable: "staff",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_MessageChat_Customer",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageChat_AdminId",
                table: "MessageChat",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageChat_UserId",
                table: "MessageChat",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageChat");
        }
    }
}
