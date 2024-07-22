namespace GISServer.Domain.Model 
{
    public class GeometryVersion
    {
        public Guid Id { get; set; }
        public String? AuthoritativeKnowledgeSource { get; set; }
        public int? Version { get; set; }
        public Status? Status { get; set; }
        public DateTime? ArchiveTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreationTime { get; set; }
        public String? BorderGeocodes { get; set; }
        public double? AreaValue { get; set; }
        public double? WestToEastLength { get; set; }
        public double? NorthToSouthLength { get; set; }
        public Guid? GeographicalObjectId { get; set; }
        public GeoObject? GeographicalObject { get; set; }
    }
}