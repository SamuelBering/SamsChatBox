

using System;

namespace DotNetGigs.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public DateTime DateTime { get; set; }
    }
}