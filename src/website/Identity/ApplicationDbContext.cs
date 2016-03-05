namespace Kcsara.Database.Website.Identity
{
  using Microsoft.AspNet.Identity.EntityFramework;
  using Microsoft.Data.Entity;

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<PendingUser> PendingUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<PendingUser>().HasAlternateKey(f => f.Email);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }
  }
}
