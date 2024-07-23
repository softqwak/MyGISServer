using GISServer.Domain.Model;

namespace GISServer.API.Model
{
    public class ClassifierDTO
    {
        public Guid Id { get; set; }
        public Status? Status { get; set; }
        public String? Name { get; set; }
        public String? Code { get; set; }
        public String? CommonInfo { get; set; }
    }
}
