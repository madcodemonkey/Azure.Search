using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSearchFilterGroupValidatorV2 : AbstractValidator<AcmeSearchFilterFieldV2>
{
    public AcmeSearchFilterGroupValidatorV2()
    {
        RuleFor(x => x.FieldName).NotEmpty();
        RuleFor(x => (int)x.PeerOperator).InclusiveBetween((int)AcmeSearchGroupOperatorEnum.And, (int)AcmeSearchGroupOperatorEnum.Or);
        RuleFor(x => (int)x.FiltersOperator).InclusiveBetween((int)AcmeSearchGroupOperatorEnum.And, (int)AcmeSearchGroupOperatorEnum.Or);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterItemValidator());
    }
}