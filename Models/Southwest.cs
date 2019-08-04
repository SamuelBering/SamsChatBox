using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class Southwest
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }

}
