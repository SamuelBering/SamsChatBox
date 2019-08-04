using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    
    [DataContract]
    public class PlusCode
    {
        [DataMember]
        public string compound_code { get; set; }
        [DataMember]
        public string global_code { get; set; }
    }

}
