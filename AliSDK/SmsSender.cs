using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Tea;

namespace AliSDK;

public static class SmsSender
{
    public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(string accessKeyId, string accessKeySecret)
    {
        AlibabaCloud.OpenApiClient.Models.Config config = new()
        {
            AccessKeyId = accessKeyId,
            AccessKeySecret = accessKeySecret
        };

        config.Endpoint = "dysmsapi.aliyuncs.com";
        return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
    }
    public static void UseSms(string phone, string token)
    {
        var client = CreateClient(accessKeyId: $@"your accessKeyId", accessKeySecret: $@"your accessKeySecret");

        SendSmsRequest sendSmsRequest = new SendSmsRequest()
        {
            PhoneNumbers = phone,
            SignName = "your SignName",
            TemplateCode = "your TemplateCode",
            TemplateParam = $"{{\"code\":\"{token}\"}}"
        };

        try
        {
            client.SendSmsWithOptionsAsync(sendSmsRequest, new AlibabaCloud.TeaUtil.Models.RuntimeOptions());
        }
        catch (TeaException error)
        {

            AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
        }
        catch (Exception e)
        {
            TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message",e.Message}
                });

            AlibabaCloud.TeaUtil.Common.AssertAsString(e.Message);
        }
    }
}
