using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migr2307_0341 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ParentChildObjectLinks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GeoObjectsClassifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GeoNamesFeatures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Classifiers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Aspects",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ParentChildObjectLinks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeoObjectsClassifiers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GeoNamesFeatures");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Classifiers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Aspects");
        }
    }
}
