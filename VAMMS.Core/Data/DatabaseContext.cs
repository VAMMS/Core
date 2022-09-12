using Microsoft.EntityFrameworkCore;
using VAMMS.Shared.Models;

namespace VAMMS.Core.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

#nullable disable

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<VisitorApplication> VisitorApplications { get; set; }
    public DbSet<WebsiteLog> WebsiteLogs { get; set; }

#nullable enable
}
