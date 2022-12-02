using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSuggestQueryValidator : AbstractValidator<AcmeSuggestorQuery>
{
    public AcmeSuggestQueryValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterGroupValidator());
    }
}