using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record SendCodeByPhoneRequest(string PhoneNumber);
public class SendCodeByPhoneRequestValidator : AbstractValidator<SendCodeByPhoneRequest>
{
    public SendCodeByPhoneRequestValidator()
    {
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
        /*RuleFor(user => user.PhoneNumber).NotNull().NotEmpty()
            .WithMessage("手机号不能为空")
            .Length(11).WithMessage("手机号长度必须为11位")
            .Matches(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")
            .WithMessage("手机号格式不正确");*/
    }
}
