using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DotNetGigs.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetGigs.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }


        //user managagement
        public DbSet<Jobseeker> Jobseekers { get; set; }

        public DbSet<Employer> Employer { get; set; }

        //Chat 
        public DbSet<Message> Messages { get; set; }

        public DbSet<Room> Rooms { get; set; }

    }
}