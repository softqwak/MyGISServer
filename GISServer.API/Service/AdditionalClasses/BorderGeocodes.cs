namespace GISServer.API.Service.AdditionalClasses
{
    public class BorderGeocodes
    {
        public String? type { get; set; }
        public List<double[]> coordinates { get; set; } = new List<double[]>();
    }
}
