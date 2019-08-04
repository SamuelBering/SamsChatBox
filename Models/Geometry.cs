using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
     [DataContract]
    public class Geometry
    {
        [DataMember]
        public Location location { get; set; }
        [DataMember]
        public Viewport viewport { get; set; }
    }

}
