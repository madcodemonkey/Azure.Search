using FluentValidation;
using Search.CogServices;
using Search.Web.Models;

namespace Search.Web;

public class AcmeSuggestorQueryDtoValidator : AbstractValidator<AcmeSuggestorQueryDto>
{
    public AcmeSuggestorQueryDtoValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterGroupValidator());
    }
}