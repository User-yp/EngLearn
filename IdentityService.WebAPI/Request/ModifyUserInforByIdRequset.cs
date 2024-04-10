using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record ModifyUserInforByIdRequset(Guid UserId, string? UserName,string? PhoneNumber,string? Password);
public class ModifyUserInforByIdRequsetValidator : AbstractValidator<ModifyUserInforByIdRequset>
{
    public ModifyUserInforByIdRequsetValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty();
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();

        /*RuleFor(e => e.UserName).NotNull().NotEmpty()
            .MaximumLength(20).MinimumLength(2)
            .WithMessage("名称长度为2-20位");

        RuleFor(user => user.PhoneNumber).NotNull().NotEmpty()
            .WithMessage("手机号不能为空")
            .Length(11).WithMessage("手机号长度必须为11位")
            .Matches(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")
            .WithMessage("手机号格式不正确");

        RuleFor(user => user.Password).NotNull().NotEmpty()
            .WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");*/
    }
}