using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vorder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingloggingforexceptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLogs");
        }
    }
}
