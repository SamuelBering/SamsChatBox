using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    [DataContract]
    public class NearbySearchResult
    {
        [DataMember]
        public List<object> html_attributions { get; set; }
        [DataMember]
        public List<Place> results { get; set; }
        [DataMember]
        public string status { get; set; }
    }

}