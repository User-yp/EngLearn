using Commons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using SearchService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Infrastructure;

internal class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IElasticClient>(sp =>
        {
            var option = sp.GetRequiredService<IOptions<ElasticSearchOptions>>();
            var settings = new ConnectionSettings(option.Value.Url);
            return new ElasticClient(settings);
        });
        services.AddScoped<ISearchRepository, SearchRepository>();
    }
}
