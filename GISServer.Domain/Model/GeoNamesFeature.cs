namespace GISServer.Domain.Model
{
    public class GeoNamesFeature
    {
       public Guid Id { get; set; }
       public String? GeoNamesFeatureCode { get; set; }
       public String? GeoNamesFeatureKind { get; set; }
       public String? FeatureKindNameEn { get; set; }
       public String? FeatureNameEn { get; set; }
       public String? FeatureKindNameRu { get; set; }
       public String? FeatureNameRu { get; set; }
       public String? CommentsEn { get; set; }
       public String? CommentsRu { get; set; }
       public List<GeoObject>? GeographicalObject { get; set; }
    }
}