using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.WebAPI.Request;

public record UserModifyInforRequset(string? UserName, string? PhoneNumber, ChangePasswordRequest? PasswordRequest);
public class UserModifyInforRequsetValidator : AbstractValidator<UserModifyInforRequset>
{
    public UserModifyInforRequsetValidator()
    {
        RuleFor(e => e.PasswordRequest.NewPassword)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.PasswordRequest.Password)) // 当旧密码不为空时，新密码不能为空
            .NotEqual(x => x.PasswordRequest.Password).When(x => !string.IsNullOrEmpty(x.PasswordRequest.Password)) // 当旧密码不为空时，新密码不能与旧密码相同
            .WithMessage("新密码不能与旧密码相同");

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
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");

        RuleFor(e => e.NewPassword).NotNull().NotEmpty()
            .WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位")
            .NotEqual(e => e.Password)
            .WithMessage("新密码不能与旧密码相同");*/
    }

}


