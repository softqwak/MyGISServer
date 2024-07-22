namespace GISServer.Domain.Model
{
    public class GeoAssociations
    {
        public Guid Id { get; set; }
        public string? Predicate { get; set; }
        public Status? Status { get; set; }
        public string? CommonBorder { get; set; }
        public bool? CompletelyIncludedFlag { get; set; }
        public double? IncludedPercent { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public Guid? GeographicalObjectInId { get; set; }
        public GeoObject? GeographicalObjectIn { get; set; }
        public Guid? GeographicalObjectOutId { get; set; }
        public GeoObject? GeographicalObjectOut { get; set; }


    }
}
