using GISServer.Domain.Model;

namespace GISServer.API.Model
{
    public class GeoObjectDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? GeoNameId { get; set; }
        public Status? Status { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreationTime { get; set; }
        public GeoNamesFeatureDTO? GeoNameFeature { get; set; }
        public GeometryVersionDTO? Geometry { get; set; }
        public List<GeometryVersionDTO>? GeometryVersion { get; set; } = new List<GeometryVersionDTO>();
        public GeoObjectInfoDTO? GeoObjectInfo { get; set; }
        public List<ParentChildObjectLinkDTO>? ParentGeoObjects { get; set; } = new List<ParentChildObjectLinkDTO>();
        public List<ParentChildObjectLinkDTO>? ChildGeoObjects { get; set; } = new List<ParentChildObjectLinkDTO>(); 
        public List<TopologyLinkDTO>? OutputTopologyLinks { get; set; } = new List<TopologyLinkDTO>();
        public List<TopologyLinkDTO>? InputTopologyLinks { get; set; } = new List<TopologyLinkDTO>();
        public List<AspectDTO>? Aspects { get; set; } = new List<AspectDTO>();        
    }
}
