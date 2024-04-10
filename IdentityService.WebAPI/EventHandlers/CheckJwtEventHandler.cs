using EventBus;

namespace IdentityService.WebAPI.Events;

[EventName("IdentityService.CheckJwt")]
public class CheckJwtEventHandler : JsonIntegrationEventHandler<CheckJwtEvent>
{
    public override Task HandleJson(string eventName, CheckJwtEvent? eventData)
    {
        //todo:前端退出登录
        var user = eventData.User;
        return Task.CompletedTask;
    }
}
