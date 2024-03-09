using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyDBConfigProvider;

public static class DBConfigurationProviderExtensions
{
    public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder,
        DBConfigOptions setup)
    {
        return
            builder.Add(new DBConfigurationSource(setup));
    }

    public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Func<IDbConnection> createDbConnection, string tableName = "T_Configs", bool reloadOnChange = false, TimeSpan? reloadInterval = null)
    {
        return AddDbConfiguration(builder, new DBConfigOptions
        {
            CreateDbConnection = createDbConnection,
            TableName = tableName,
            ReloadOnChange = reloadOnChange,
            ReloadInterval = reloadInterval
        });
    }
}
