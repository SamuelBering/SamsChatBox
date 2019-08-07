using System.ComponentModel.DataAnnotations;

namespace DotNetGigs.Models
{
    public class PlaceFilter
    {
        [Required]
        public double? Lat { get; set; }
        [Required]
        public double? Long { get; set; }
        [Required]
        public double? Radius { get; set; }
        public string Keyword { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
    }
}