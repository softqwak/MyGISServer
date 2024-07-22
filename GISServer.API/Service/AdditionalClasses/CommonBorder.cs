namespace GISServer.API.Service.AdditionalClasses
{
    public class CommonBorder
    {
        public String? type { get; set; }
        public List<double[]> coordinates { get; set; } = new List<double[]>();
    }
}
