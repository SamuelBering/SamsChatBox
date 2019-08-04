using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetGigs.Models
{
    
    [DataContract]
    public class Place
    {
        [DataMember]
        public Geometry geometry { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public OpeningHours opening_hours { get; set; }
        [DataMember]
        public List<Photo> photos { get; set; }
        [DataMember]
        public string place_id { get; set; }
        [DataMember]
        public PlusCode plus_code { get; set; }
        [DataMember]
        public int price_level { get; set; }
        [DataMember]
        public double rating { get; set; }
        [DataMember]
        public string reference { get; set; }
        [DataMember]
        public string scope { get; set; }
        [DataMember]
        public List<string> types { get; set; }
        [DataMember]
        public int user_ratings_total { get; set; }
        [DataMember]
        public string vicinity { get; set; }
    }

}
