using System.Data.Entity;

namespace GunStore.Models
{
    public class ComicsContextDb : DbContext
    {
        public ComicsContextDb():base("name=DefaultConnection")
        {

        }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Gun> Guns { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}