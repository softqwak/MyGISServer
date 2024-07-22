using GISServer.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace GISServer.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public Context()
        {
        }

        public DbSet<GeoObject> GeoObjects { get; set; }
        public DbSet<Classifier> Classifiers { get; set; }
        public DbSet<GeoNamesFeature> GeoNamesFeatures { get; set; }
        public DbSet<GeometryVersion> GeometryVersions { get; set; }
        public DbSet<GeoObjectInfo> GeoObjectInfos { get; set; }
        public DbSet<ParentChildObjectLink> ParentChildObjectLinks { get; set; }
        public DbSet<TopologyLink> TopologyLinks { get; set; }  
        public DbSet<GeoObjectsClassifiers> GeoObjectsClassifiers { get; set; }
        public DbSet<Aspect> Aspects { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseNpgsql("Host=localhost;Port=5432;Database=gisserver;Username=postgres;Password=root;",
                o => o.UseNetTopologySuite());
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GeoObject>()
                .HasOne(e => e.GeoNameFeature)
                .WithMany(e => e.GeographicalObject)
                .HasForeignKey(e => e.GeoNameFeatureId);
            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.GeometryVersion)
                .WithOne(e => e.GeographicalObject)
                .HasForeignKey(e => e.GeographicalObjectId);
            modelBuilder
                .Entity<GeoObject>()
                .HasOne(e => e.GeoObjectInfo)
                .WithOne(e => e.GeographicalObject)
                .HasForeignKey<GeoObjectInfo>(e => e.GeographicalObjectId);
            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.ParentGeoObjects)
                .WithOne(e => e.ChildGeographicalObject)
                .HasForeignKey(e => e.ChildGeographicalObjectId);
            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.ChildGeoObjects)
                .WithOne(e => e.ParentGeographicalObject)
                .HasForeignKey(e => e.ParentGeographicalObjectId);
            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.InputTopologyLinks)
                .WithOne(e => e.GeographicalObjectIn)
                .HasForeignKey(e => e.GeographicalObjectInId);
            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.OutputTopologyLinks)
                .WithOne(e => e.GeographicalObjectOut)
                .HasForeignKey(e => e.GeographicalObjectOutId);

            modelBuilder 
                .Entity<GeoObjectsClassifiers>()
                .HasKey(e => new {e.ClassifierId, e.GeoObjectId});
            modelBuilder.Entity<GeoObjectsClassifiers>()
                .HasOne<GeoObjectInfo>(e => e.GeoObjectInfo)
                .WithMany(e => e.GeoObjectsClassifiers)
                .HasForeignKey(e => e.GeoObjectId);
            modelBuilder.Entity<GeoObjectsClassifiers>()
                .HasOne<Classifier>(e => e.Classifier)
                .WithMany(e => e.GeoObjectsClassifiers)
                .HasForeignKey(e => e.ClassifierId);

            modelBuilder
                .Entity<GeoObject>()
                .HasMany(e => e.Aspects)
                .WithOne(e => e.GeographicalObject)
                .HasForeignKey(e => e.GeographicalObjectId);
        }
    }
}
