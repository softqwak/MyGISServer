using GISServer.Domain.Model;

namespace GISServer.API.Model
{
    public class GeoNamesFeatureDTO
    {
       public Guid? Id { get; set; }
       public Status? Status { get; set; }
       public String? GeoNamesFeatureCode { get; set; }
       public String? GeoNamesFeatureKind { get; set; }
       public String? FeatureKindNameEn { get; set; }
       public String? FeatureNameEn { get; set; }
       public String? FeatureKindNameRu { get; set; }
       public String? FeatureNameRu { get; set; }
       public String? CommentsEn { get; set; }
       public String? CommentsRu { get; set; }
    }
}