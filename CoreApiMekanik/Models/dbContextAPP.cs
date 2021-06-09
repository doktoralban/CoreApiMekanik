using Microsoft.EntityFrameworkCore;
 

namespace CoreApiMekanik.Models
{
    public class dbContextAPP:DbContext
    {
        public dbContextAPP(DbContextOptions<dbContextAPP> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }





    }
}
