using Microsoft.EntityFrameworkCore;
using EdgeWEB.Models;

namespace EdgeWEB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

 
    }
}
