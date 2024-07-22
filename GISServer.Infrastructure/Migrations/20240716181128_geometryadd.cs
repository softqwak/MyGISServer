using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class geometryadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GeometryId",
                table: "GeoObjects",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjects_GeometryId",
                table: "GeoObjects",
                column: "GeometryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoObjects_GeometryVersions_GeometryId",
                table: "GeoObjects",
                column: "GeometryId",
                principalTable: "GeometryVersions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoObjects_GeometryVersions_GeometryId",
                table: "GeoObjects");

            migrationBuilder.DropIndex(
                name: "IX_GeoObjects_GeometryId",
                table: "GeoObjects");

            migrationBuilder.DropColumn(
                name: "GeometryId",
                table: "GeoObjects");
        }
    }
}
