using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record LoginByUserNameAndPwdRequest(string UserName, string Password);
public class LoginByUserNameAndPwdRequestValidator : AbstractValidator<LoginByUserNameAndPwdRequest>
{
    public LoginByUserNameAndPwdRequestValidator()
    {
        RuleFor(e => e.UserName).NotNull().NotEmpty();
        RuleFor(e => e.Password).NotNull().NotEmpty();

        /*RuleFor(e => e.UserName).NotEmpty()
            .MaximumLength(20).MinimumLength(2)
            .WithMessage("名称长度为2-20位");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");*/
    }
}
