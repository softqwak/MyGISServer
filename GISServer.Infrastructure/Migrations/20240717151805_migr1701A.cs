using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr1701A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoAspect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    EndPoint = table.Column<string>(type: "text", nullable: true),
                    CommonInfo = table.Column<string>(type: "text", nullable: true),
                    GeoObjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoAspect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoAspect_GeoObjects_GeoObjectId",
                        column: x => x.GeoObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeoAspect_GeoObjectId",
                table: "GeoAspect",
                column: "GeoObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoAspect");
        }
    }
}
