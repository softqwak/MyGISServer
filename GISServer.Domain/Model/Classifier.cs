
namespace GISServer.Domain.Model
{
    public class Classifier
    {
        public Guid Id { get; set; }
        public String? Name { get; set; }
        public String? Code { get; set; }
        public String? CommonInfo { get; set; }
        public Status? Status { get; set; }
        public List<GeoObjectsClassifiers> GeoObjectsClassifiers { get; set; } = new List<GeoObjectsClassifiers>();

    }
}
