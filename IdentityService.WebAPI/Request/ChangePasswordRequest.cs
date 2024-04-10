using FluentValidation;

namespace IdentityService.WebAPI.Request;

public record ChangePasswordRequest(string Password, string NewPassword);
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(e => e.Password).NotNull().NotEmpty();
        RuleFor(e => e.NewPassword).NotNull().NotEmpty()
            .NotEqual(e => e.Password)
            .WithMessage("两次密码不能一致");
        /*RuleFor(user => user.Password)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位");

        RuleFor(user => user.NewPassword)
            .NotEmpty().WithMessage("密码不能为空")
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$")
            .WithMessage("密码必须包含数字、大小写字母和至少一个特殊字符，且长度至少为8位")
            .NotEqual(e => e.Password)
            .WithMessage("两次密码不能一致");*/
    }
}
