using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GISServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "GeoNamesFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GeoNamesFeatureCode = table.Column<string>(type: "text", nullable: true),
                    GeoNamesFeatureKind = table.Column<string>(type: "text", nullable: true),
                    FeatureKindNameEn = table.Column<string>(type: "text", nullable: true),
                    FeatureNameEn = table.Column<string>(type: "text", nullable: true),
                    FeatureKindNameRu = table.Column<string>(type: "text", nullable: true),
                    FeatureNameRu = table.Column<string>(type: "text", nullable: true),
                    CommentsEn = table.Column<string>(type: "text", nullable: true),
                    CommentsRu = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoNamesFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    GeoNameId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GeoNameFeatureId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoObjects_GeoNamesFeatures_GeoNameFeatureId",
                        column: x => x.GeoNameFeatureId,
                        principalTable: "GeoNamesFeatures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeometryVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthoritativeKnowledgeSource = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    ArchiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BorderGeocodes = table.Column<string>(type: "text", nullable: true),
                    AreaValue = table.Column<double>(type: "double precision", nullable: true),
                    WestToEastLength = table.Column<double>(type: "double precision", nullable: true),
                    NorthToSouthLength = table.Column<double>(type: "double precision", nullable: true),
                    GeographicalObjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeometryVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeometryVersions_GeoObjects_GeographicalObjectId",
                        column: x => x.GeographicalObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeoObjectInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ShortName = table.Column<string>(type: "text", nullable: true),
                    AuthoritativeKnowledgeSource = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    ArchiveTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CommonInfo = table.Column<string>(type: "text", nullable: true),
                    GeographicalObjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoObjectInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoObjectInfos_GeoObjects_GeographicalObjectId",
                        column: x => x.GeographicalObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParentChildObjectLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentGeographicalObjectName = table.Column<string>(type: "text", nullable: true),
                    ChildGeographicalObjectName = table.Column<string>(type: "text", nullable: true),
                    CompletelyIncludedFlag = table.Column<bool>(type: "boolean", nullable: true),
                    IncludedPercent = table.Column<double>(type: "double precision", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ParentGeographicalObjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChildGeographicalObjectId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChildObjectLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentChildObjectLinks_GeoObjects_ChildGeographicalObjectId",
                        column: x => x.ChildGeographicalObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentChildObjectLinks_GeoObjects_ParentGeographicalObjectId",
                        column: x => x.ParentGeographicalObjectId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TopologyLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Predicate = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CommonBorder = table.Column<string>(type: "text", nullable: true),
                    GeographicalObjectInId = table.Column<Guid>(type: "uuid", nullable: true),
                    GeographicalObjectOutId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopologyLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopologyLinks_GeoObjects_GeographicalObjectInId",
                        column: x => x.GeographicalObjectInId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TopologyLinks_GeoObjects_GeographicalObjectOutId",
                        column: x => x.GeographicalObjectOutId,
                        principalTable: "GeoObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeometryVersions_GeographicalObjectId",
                table: "GeometryVersions",
                column: "GeographicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjectInfos_GeographicalObjectId",
                table: "GeoObjectInfos",
                column: "GeographicalObjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeoObjects_GeoNameFeatureId",
                table: "GeoObjects",
                column: "GeoNameFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildObjectLinks_ChildGeographicalObjectId",
                table: "ParentChildObjectLinks",
                column: "ChildGeographicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentChildObjectLinks_ParentGeographicalObjectId",
                table: "ParentChildObjectLinks",
                column: "ParentGeographicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TopologyLinks_GeographicalObjectInId",
                table: "TopologyLinks",
                column: "GeographicalObjectInId");

            migrationBuilder.CreateIndex(
                name: "IX_TopologyLinks_GeographicalObjectOutId",
                table: "TopologyLinks",
                column: "GeographicalObjectOutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeometryVersions");

            migrationBuilder.DropTable(
                name: "GeoObjectInfos");

            migrationBuilder.DropTable(
                name: "ParentChildObjectLinks");

            migrationBuilder.DropTable(
                name: "TopologyLinks");

            migrationBuilder.DropTable(
                name: "GeoObjects");

            migrationBuilder.DropTable(
                name: "GeoNamesFeatures");
        }
    }
}
