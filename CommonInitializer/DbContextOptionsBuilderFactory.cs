using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInitializer;

public static class DbContextOptionsBuilderFactory
{
    public static DbContextOptionsBuilder<TDbContext> Create<TDbContext>() where TDbContext : DbContext
    {
        var connStr = Environment.GetEnvironmentVariable("DefaultDB:ConnStr");
        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();
        optionsBuilder.UseSqlServer(connStr);
        return optionsBuilder;
    }
}
