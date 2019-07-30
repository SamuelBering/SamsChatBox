using System;

namespace DotNetGigs.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public string SenderId { get; set; }  //maps to identity of sender
        public AppUser Sender { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}