using GISServer.Domain.Model;

namespace GISServer.API.Model
{
    public class GeoObjectsClassifiersDTO
    {
        public Guid? GeoObjectId { get; set; }
        public Guid? ClassifierId { get; set; }
    }
}
