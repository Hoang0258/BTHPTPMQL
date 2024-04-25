using Microsoft.EntityFrameworkCore;
using MvcHoang.Models;

namespace MvcHoang.Data
{
 public class ApplicationDbContext : DbContext
{
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {}
    public DbSet<Person> Person { get; set;}
    
}
}