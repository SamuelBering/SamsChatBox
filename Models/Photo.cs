using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class Photo
    {
        [DataMember]
        public int height { get; set; }
        [DataMember]
        public List<string> html_attributions { get; set; }
        [DataMember]
        public string photo_reference { get; set; }
        [DataMember]
        public int width { get; set; }
    }
}
