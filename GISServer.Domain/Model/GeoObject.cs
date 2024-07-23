namespace GISServer.Domain.Model
{
    public class GeoObject
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? GeoNameId { get; set; }
        public Status? Status { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreationTime { get; set; }
        public Guid? GeoNameFeatureId { get; set; }
        public GeoNamesFeature? GeoNameFeature { get; set; }
        public GeometryVersion? Geometry { get; set; }
        public List<GeometryVersion>? GeometryVersion { get; set; } = new List<GeometryVersion>();
        public GeoObjectInfo? GeoObjectInfo { get; set; }
        public List<ParentChildObjectLink>? ParentGeoObjects { get; set; } = new List<ParentChildObjectLink>();
        public List<ParentChildObjectLink>? ChildGeoObjects { get; set; } = new List<ParentChildObjectLink>(); 
        public List<TopologyLink>? OutputTopologyLinks { get; set; } = new List<TopologyLink>();
        public List<TopologyLink>? InputTopologyLinks { get; set; } = new List<TopologyLink>();
        public List<Aspect>? Aspects { get; set; } = new List<Aspect>();        

    }
}

