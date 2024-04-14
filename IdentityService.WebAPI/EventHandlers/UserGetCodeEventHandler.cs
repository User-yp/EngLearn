using ASPNETCore.RedisService;
using EventBus;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace IdentityService.WebAPI.Events;

[EventName("IdentityService.GetCode")]
public class UserGetCodeEventHandler : JsonIntegrationEventHandler<UserGetCodeEvent>
{
    private readonly ILogger<UserGetCodeEventHandler> logger;
    private readonly IRedisHelper redis;

    public UserGetCodeEventHandler(ILogger<UserGetCodeEventHandler> logger,IRedisHelper redis)
    {
        this.logger = logger;
        this.redis = redis;
    }

    public override Task HandleJson(string eventName, UserGetCodeEvent? eventData)
    {
        redis.HashSetorCreateFieldsAsync(nameof(UserGetCodeEvent), new ConcurrentDictionary<string, string>
        {
            [eventData.PhoneNum] = eventData.Code.ToString()
        });

        return Task.CompletedTask;
    }
}
