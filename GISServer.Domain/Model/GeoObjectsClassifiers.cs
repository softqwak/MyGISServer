using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISServer.Domain.Model
{
    public class GeoObjectsClassifiers
    {
        public Classifier? Classifier { get; set; }
        public GeoObjectInfo? GeoObjectInfo { get; set; }
        public Guid? GeoObjectId { get; set; }
        public Guid? ClassifierId { get; set; }
        public Status? Status { get; set; }
    }
}
 
