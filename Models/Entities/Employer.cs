namespace DotNetGigs.Models.Entities
{
    public class Employer  
    {
        public int Id { get; set; }     
        public string IdentityId { get; set; }   
        public AppUser Identity { get; set; }  // navigation property
        public string Company {get;set;}
    }
}