using EventBus;
using IdentityService.WebAPI.Request;
using Newtonsoft.Json;

namespace IdentityService.WebAPI.Events;

[EventName("IdentityService.User.SmsCodeSignUp")]
public class UserSmsCodeSignUpEventHandler : JsonIntegrationEventHandler<UserSmsCodeSignUpEvent>
{
    private readonly ILogger<ResetPasswordEventHandler> logger;

    public UserSmsCodeSignUpEventHandler(ILogger<ResetPasswordEventHandler> logger)
    {
        this.logger = logger;
    }
    public override async Task HandleJson(string eventName, UserSmsCodeSignUpEvent? eventData)
    {
        logger.LogInformation($"{eventData.User.UserName} SignUp UserMessage DateTime{DateTime.Now}");
    }
}
