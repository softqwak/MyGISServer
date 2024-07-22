using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr1207 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoClassifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<int>(type: "integer", nullable: true),
                    CommonInfo = table.Column<string>(type: "text", nullable: true),
                    GeoObjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoClassifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoClassifiers_GeoObjects_GeoObjectId",
                        column: x => x.GeoObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoObjectsGeoClassifiers",
                columns: table => new
                {
                    GeoObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeoClassifierId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoObjectsGeoClassifiers", x => new { x.GeoClassifierId, x.GeoObjectId });
                    table.ForeignKey(
                        name: "FK_GeoObjectsGeoClassifiers_GeoClassifiers_GeoClassifierId",
                        column: x => x.GeoClassifierId,
                        principalTable: "GeoClassifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoObjectsGeoClassifiers_GeoObjects_GeoObjectId",
                        column: x => x.GeoObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeoClassifiers_GeoObjectId",
                table: "GeoClassifiers",
                column: "GeoObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjectsGeoClassifiers_GeoObjectId",
                table: "GeoObjectsGeoClassifiers",
                column: "GeoObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoObjectsGeoClassifiers");

            migrationBuilder.DropTable(
                name: "GeoClassifiers");
        }
    }
}
