using CommonInitializer;
using Listening.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityService.WebAPI;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ListeningDbContext>
{
    public ListeningDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = DbContextOptionsBuilderFactory.Create<ListeningDbContext>();
        return new ListeningDbContext(optionsBuilder.Options);
    }
}
