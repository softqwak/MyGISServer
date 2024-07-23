namespace GISServer.Domain.Model
{
    public class ParentChildObjectLink
    {
        public Guid Id { get; set; }
        public Status? Status { get; set; }
        public String? ParentGeographicalObjectName { get; set; }
        public String? ChildGeographicalObjectName { get; set; }
        public bool? CompletelyIncludedFlag { get; set; }
        public double? IncludedPercent { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public Guid? ParentGeographicalObjectId { get; set; }
        public GeoObject? ParentGeographicalObject { get; set; }
        public Guid? ChildGeographicalObjectId { get; set; }
        public GeoObject? ChildGeographicalObject { get; set; }

    }
}