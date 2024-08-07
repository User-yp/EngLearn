﻿using ASPNETCore.RedisService;
using Commons;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETCore;

class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();
        services.AddScoped<IDistributedCacheHelper, DistributedCacheHelper>();
        services.AddSingleton<IRedisHelper, RedisHelper>();
    }
}
