namespace GISServer.API.Model
{
    public class AspectDTO
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }
        public string? Code { get; set; }
        public string? EndPoint { get; set; }
        public string? CommonInfo { get; set; }
        public Guid? GeographicalObjectId { get; set; }
    }
}
