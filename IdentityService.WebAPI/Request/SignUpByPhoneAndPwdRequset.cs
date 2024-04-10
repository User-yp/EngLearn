using FluentValidation;
using IdentityService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.WebAPI.Request;

public record SignUpByPhoneAndPwdRequset(string UserName,string PhoneNumber,string Password,string ConfirmPassword, string Smscode);
public class UserSignUpByPhoneAndPwdRequsetValidator: AbstractValidator<SignUpByPhoneAndPwdRequset>
{
    public UserSignUpByPhoneAndPwdRequsetValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty();
        RuleFor(e => e.PhoneNumber).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.ConfirmPassword).NotNull().NotEmpty();
        RuleFor(e => e.Smscode).NotNull().NotEmpty();
        /*RuleFor(e => e.UserName).NotEmpty()
            .MaximumLength(20).MinimumLength(2)
            .WithMessage("名称长度为2-20位");

        RuleFor(user => user.PhoneNumber).NotNull().NotEmpty()
            .WithMessage("手机号不能为空")
            .Length(11).WithMessage("手机号长度必须为11位")
            .Matches(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")
            .WithMessage("手机号格式不正确");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");

        RuleFor(user => user.ConfirmPassword)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位")
            .Equal(user => user.Password)
            .WithMessage("两次密码必须一致");

        RuleFor(e => e.Smscode).NotNull().NotEmpty()
            .WithMessage("验证码不能为空")
            .Matches(@"^\d{6}$").WithMessage("验证码必须是六位数字");*/

    }
}
