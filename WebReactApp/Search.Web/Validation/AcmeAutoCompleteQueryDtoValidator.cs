using FluentValidation;
using Search.Web.Models;

namespace Search.Web;

public class AcmeAutoCompleteQueryDtoValidator : AbstractValidator<AcmeAutoCompleteQueryDto>
{
    public AcmeAutoCompleteQueryDtoValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterFieldValidator());
    }
}