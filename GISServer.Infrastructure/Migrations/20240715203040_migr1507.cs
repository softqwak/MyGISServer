using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr1507 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoClassifiers_GeoObjects_GeoObjectId",
                table: "GeoClassifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_GeoObjectsGeoClassifiers_GeoObjects_GeoObjectId",
                table: "GeoObjectsGeoClassifiers");

            migrationBuilder.RenameColumn(
                name: "GeoObjectId",
                table: "GeoClassifiers",
                newName: "GeoObjectInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_GeoClassifiers_GeoObjectId",
                table: "GeoClassifiers",
                newName: "IX_GeoClassifiers_GeoObjectInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoClassifiers_GeoObjectInfos_GeoObjectInfoId",
                table: "GeoClassifiers",
                column: "GeoObjectInfoId",
                principalTable: "GeoObjectInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoObjectsGeoClassifiers_GeoObjectInfos_GeoObjectId",
                table: "GeoObjectsGeoClassifiers",
                column: "GeoObjectId",
                principalTable: "GeoObjectInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoClassifiers_GeoObjectInfos_GeoObjectInfoId",
                table: "GeoClassifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_GeoObjectsGeoClassifiers_GeoObjectInfos_GeoObjectId",
                table: "GeoObjectsGeoClassifiers");

            migrationBuilder.RenameColumn(
                name: "GeoObjectInfoId",
                table: "GeoClassifiers",
                newName: "GeoObjectId");

            migrationBuilder.RenameIndex(
                name: "IX_GeoClassifiers_GeoObjectInfoId",
                table: "GeoClassifiers",
                newName: "IX_GeoClassifiers_GeoObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoClassifiers_GeoObjects_GeoObjectId",
                table: "GeoClassifiers",
                column: "GeoObjectId",
                principalTable: "GeoObjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoObjectsGeoClassifiers_GeoObjects_GeoObjectId",
                table: "GeoObjectsGeoClassifiers",
                column: "GeoObjectId",
                principalTable: "GeoObjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
