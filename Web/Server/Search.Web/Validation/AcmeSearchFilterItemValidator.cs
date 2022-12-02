using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSearchFilterItemValidator : AbstractValidator<AcmeSearchFilterItem>
{
    public AcmeSearchFilterItemValidator()
    {
        RuleFor(x => (int)x.Operator).InclusiveBetween((int)AcmeSearchFilterOperatorEnum.Equal, (int)AcmeSearchFilterOperatorEnum.WithinRange);
    }
}