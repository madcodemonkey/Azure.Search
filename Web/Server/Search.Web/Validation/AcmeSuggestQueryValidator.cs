using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSuggestQueryValidator : AbstractValidator<AcmeSuggestQuery>
{
    public AcmeSuggestQueryValidator()
    {
        RuleFor(x => x.IndexName).NotEmpty();
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleFor(x => x.SuggestorName).NotEmpty();
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterFieldValidator());
    }
}