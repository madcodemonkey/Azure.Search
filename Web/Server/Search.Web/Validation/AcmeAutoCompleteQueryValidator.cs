using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeAutoCompleteQueryValidator : AbstractValidator<AcmeAutoCompleteQuery>
{
    public AcmeAutoCompleteQueryValidator()
    {
        RuleFor(x => x.IndexName).NotEmpty();
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleFor(x => x.SuggestorName).NotEmpty();
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterFieldValidator());
    }
}