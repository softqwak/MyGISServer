using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr1807 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoObjectsGeoClassifiers");

            migrationBuilder.DropTable(
                name: "GeoClassifiers");

            migrationBuilder.CreateTable(
                name: "Classifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CommonInfo = table.Column<string>(type: "text", nullable: true),
                    GeoObjectInfoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classifiers_GeoObjectInfos_GeoObjectInfoId",
                        column: x => x.GeoObjectInfoId,
                        principalTable: "GeoObjectInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoObjectsClassifiers",
                columns: table => new
                {
                    GeoObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassifierId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoObjectsClassifiers", x => new { x.ClassifierId, x.GeoObjectId });
                    table.ForeignKey(
                        name: "FK_GeoObjectsClassifiers_Classifiers_ClassifierId",
                        column: x => x.ClassifierId,
                        principalTable: "Classifiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeoObjectsClassifiers_GeoObjectInfos_GeoObjectId",
                        column: x => x.GeoObjectId,
                        principalTable: "GeoObjectInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classifiers_GeoObjectInfoId",
                table: "Classifiers",
                column: "GeoObjectInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjectsClassifiers_GeoObjectId",
                table: "GeoObjectsClassifiers",
                column: "GeoObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoObjectsClassifiers");

            migrationBuilder.DropTable(
                name: "Classifiers");

            migrationBuilder.CreateTable(
                name: "GeoClassifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CommonInfo = table.Column<string>(type: "text", nullable: true),
                    GeoObjectInfoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoClassifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoClassifiers_GeoObjectInfos_GeoObjectInfoId",
                        column: x => x.GeoObjectInfoId,
                        principalTable: "GeoObjectInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoObjectsGeoClassifiers",
                columns: table => new
                {
                    GeoClassifierId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeoObjectId = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "FK_GeoObjectsGeoClassifiers_GeoObjectInfos_GeoObjectId",
                        column: x => x.GeoObjectId,
                        principalTable: "GeoObjectInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeoClassifiers_GeoObjectInfoId",
                table: "GeoClassifiers",
                column: "GeoObjectInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjectsGeoClassifiers_GeoObjectId",
                table: "GeoObjectsGeoClassifiers",
                column: "GeoObjectId");
        }
    }
}
