using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record AddAdminUserRequest(string UserName, string PhoneNum);
public class AddAdminUserRequestValidator : AbstractValidator<AddAdminUserRequest>
{
    public AddAdminUserRequestValidator()
    {
        RuleFor(e => e.PhoneNum).NotNull().NotEmpty().MaximumLength(11);
        RuleFor(e => e.UserName).NotEmpty().NotEmpty().MaximumLength(20).MinimumLength(2);
        /*RuleFor(e => e.UserName).NotEmpty()
            .MaximumLength(20).MinimumLength(2)
            .WithMessage("名称长度为2-20位");
        RuleFor(user => user.PhoneNum).NotNull().NotEmpty()
            .WithMessage("手机号不能为空")
            .Length(11).WithMessage("手机号长度必须为11位")
            .Matches(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")
            .WithMessage("手机号格式不正确");*/
    }
}
