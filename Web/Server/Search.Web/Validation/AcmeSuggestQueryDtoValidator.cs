using FluentValidation;
using Search.Web.Models;

namespace Search.Web;

public class AcmeSuggestQueryDtoValidator : AbstractValidator<AcmeSuggestQueryDto>
{
    public AcmeSuggestQueryDtoValidator()
    {
        RuleFor(x => x.Query).NotEmpty();
        RuleFor(x => x.NumberOfSuggestionsToRetrieve).InclusiveBetween(1, 100);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterFieldValidator());
    }
}