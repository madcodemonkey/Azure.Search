using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSearchFilterItemValidator : AbstractValidator<AcmeSearchFilterItem>
{
    public AcmeSearchFilterItemValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => (int)x.Operator).InclusiveBetween((int)AcmeSearchFilterOperatorEnum.Equal, (int)AcmeSearchFilterOperatorEnum.WithinRange);
    }
}