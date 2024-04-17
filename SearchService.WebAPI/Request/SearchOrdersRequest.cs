using FluentValidation;

namespace SearchService.WebAPI.Request;

public record SearchOrdersRequest(string Keyword, int PageIndex, int PageSize);
public class SearchOrdersRequestValidator : AbstractValidator<SearchOrdersRequest>
{
    public SearchOrdersRequestValidator()
    {
        RuleFor(e => e.Keyword).NotNull().MinimumLength(2).MaximumLength(100);
        RuleFor(e => e.PageIndex).GreaterThan(0);//页号从1开始
        RuleFor(e => e.PageSize).GreaterThanOrEqualTo(5);
    }
}
