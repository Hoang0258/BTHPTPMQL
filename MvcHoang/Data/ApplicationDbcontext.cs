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
    public DbSet<DaiLy> DaiLy { get; set;}
  
    public DbSet<HeThongPhanPhoi> HeThongPhanPhoi { get; set;}
    public DbSet<Student> Student { get; set;}
}
}