using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class Location
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }

}
