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


        public void Union(GeoObject geo1, GeoObject geo2)
        {
            geo1.Status = Model.Status.Archive;
            geo1.InputTopologyLinks[0].Status = Model.Status.Archive;
            geo1.OutputTopologyLinks[0].Status = Model.Status.Archive;

            geo2.Status = Model.Status.Archive;
            geo2.InputTopologyLinks[0].Status = Model.Status.Archive;
            geo2.OutputTopologyLinks[0].Status = Model.Status.Archive;

        }
        public void ToString()
        {
            Console.WriteLine(Id);
            Console.WriteLine(Name);
            Console.WriteLine(GeoNameId);
            Console.WriteLine(Status);
            Console.WriteLine(GeoNameFeatureId);
            Console.WriteLine(GeoNameFeature.FeatureKindNameRu);
            Console.WriteLine(GeoNameId);
            foreach(TopologyLink link in OutputTopologyLinks)
                Console.WriteLine(link.Id);
            foreach(TopologyLink link in InputTopologyLinks)
                Console.WriteLine(link.Id);
        }
    }
}

