namespace GISServer.Domain.Model
{
    public class TopologyLink
    {
        public Guid Id { get; set; }
        public String? Predicate { get; set; }
        public Status? Status { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public String? CommonBorder { get; set; }
        public Guid? GeographicalObjectInId { get; set; }
        public GeoObject? GeographicalObjectIn { get; set; }
        public Guid? GeographicalObjectOutId { get; set; }
        public GeoObject? GeographicalObjectOut { get; set; }
    }
}