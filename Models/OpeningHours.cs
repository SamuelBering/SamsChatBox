using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class OpeningHours
    {
        [DataMember]
        public bool open_now { get; set; }
    }
    
}
