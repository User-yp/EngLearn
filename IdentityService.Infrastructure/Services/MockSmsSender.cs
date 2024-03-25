using IdentityService.Domain;
using Microsoft.Extensions.Logging;
using AliSDK;

namespace IdentityService.Infrastructure.Services;

public class MockSmsSender : ISmsSender
{
    private readonly ILogger<MockSmsSender> logger;
    public MockSmsSender(ILogger<MockSmsSender> logger)
    {
        this.logger = logger;
    }
    public Task SendAsync(string phoneNum, string code)
    {
        SmsSender.UseSms(phoneNum, code);
        logger.LogInformation($"向手机号{phoneNum}发送验证码：{code}");
        return Task.CompletedTask;
    }
}
