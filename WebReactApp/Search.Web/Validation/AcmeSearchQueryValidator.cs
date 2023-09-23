using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSearchQueryValidator : AbstractValidator<AcmeSearchQuery>
{
    public AcmeSearchQueryValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.IndexName).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.ItemsPerPage).GreaterThan(0);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterFieldValidator());
    }
}