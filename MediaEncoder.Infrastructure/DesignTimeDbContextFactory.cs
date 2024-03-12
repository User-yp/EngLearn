using CommonInitializer;
using MediaEncoder.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.WebAPI;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MEDbContext>
{
    public MEDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<MEDbContext>();
        return new MEDbContext(optionsBuilder.Options);
    }
}
