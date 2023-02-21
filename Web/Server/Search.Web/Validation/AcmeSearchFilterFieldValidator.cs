using FluentValidation;
using Search.CogServices;

namespace Search.Web;

public class AcmeSearchFilterFieldValidator : AbstractValidator<AcmeSearchFilterField>
{
    public AcmeSearchFilterFieldValidator()
    {
        RuleFor(x => x.FieldName).NotEmpty();
        RuleFor(x => (int)x.FieldType).InclusiveBetween((int)AcmeSearchFilterFieldTypeEnum.String, (int)AcmeSearchFilterFieldTypeEnum.StringCollection)
            .WithMessage($"Your filter must have a field type between {(int)AcmeSearchFilterFieldTypeEnum.String} and {(int)AcmeSearchFilterFieldTypeEnum.StringCollection} (see {nameof(AcmeSearchFilterFieldTypeEnum)})");
        RuleFor(x => (int)x.PeerOperator).InclusiveBetween((int)AcmeSearchGroupOperatorEnum.And, (int)AcmeSearchGroupOperatorEnum.Or);
        RuleFor(x => (int)x.FiltersOperator).InclusiveBetween((int)AcmeSearchGroupOperatorEnum.And, (int)AcmeSearchGroupOperatorEnum.Or);
        RuleForEach(x => x.Filters).SetValidator(new AcmeSearchFilterItemValidator());
    }
}