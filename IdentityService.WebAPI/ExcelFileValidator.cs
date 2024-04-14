using FluentValidation;

namespace IdentityService.WebAPI;

public class ExcelFileValidator : AbstractValidator<IFormFile>
{
    public ExcelFileValidator()
    {
        RuleFor(file => file.ContentType).Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            .WithMessage("只允许上传Excel文件");
    }
}
