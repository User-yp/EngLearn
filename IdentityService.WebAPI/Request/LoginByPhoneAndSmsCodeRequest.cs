using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record LoginByPhoneAndSmsCodeRequest(string PhoneNum, string SmsCode);
public class LoginByPhoneAndSmsCodeRequestValidator : AbstractValidator<LoginByPhoneAndSmsCodeRequest>
{
    public LoginByPhoneAndSmsCodeRequestValidator()
    {
        RuleFor(e => e.PhoneNum).NotNull().NotEmpty().MaximumLength(11);
        RuleFor(e => e.SmsCode).NotEmpty().NotEmpty().Length(6);

        /*RuleFor(user => user.PhoneNum).NotNull().NotEmpty()
            .WithMessage("手机号不能为空")
            .Length(11).WithMessage("手机号长度必须为11位")
            .Matches(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")
            .WithMessage("手机号格式不正确");

        RuleFor(e => e.SmsCode).NotNull().NotEmpty()
            .WithMessage("验证码不能为空")
            .Matches(@"^\d{6}$").WithMessage("验证码必须是六位数字");*/
    }
}

