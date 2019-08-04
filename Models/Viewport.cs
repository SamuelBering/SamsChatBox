using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class Viewport
    {
        [DataMember]
        public Northeast northeast { get; set; }
        [DataMember]
        public Southwest southwest { get; set; }
    }

}
