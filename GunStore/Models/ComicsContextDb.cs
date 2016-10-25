using System.Data.Entity;

namespace ComicBookStore.Models
{
    public class ComicsContextDb : DbContext
    {
        public ComicsContextDb():base("name=DefaultConnection")
        {

        }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Comics> Comics { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}