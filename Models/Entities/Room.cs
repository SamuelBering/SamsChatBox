using System.Collections.Generic;

namespace DotNetGigs.Models.Entities
{
    public class Room  
    {
        public int Id { get; set; }     
        public string Title { get; set; }  
        ICollection<Message> Messages { get; set; } //Navigation property    
    }
}