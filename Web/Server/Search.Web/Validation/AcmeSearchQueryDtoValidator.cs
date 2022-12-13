using FluentValidation;
using Search.Web.Models;

namespace Search.Web;

public class AcmeSearchQueryDtoValidator : AbstractValidator<AcmeSearchQueryDto>
{
    public AcmeSearchQueryDtoValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterGroupValidator());
    }
}