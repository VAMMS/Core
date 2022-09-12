using FluentValidation;
using VAMMS.Shared.Dtos;

namespace VAMMS.Core.Validators;

public class VisitorApplicationValidator : AbstractValidator<VisitorApplicationDto>
{
    public VisitorApplicationValidator()
    {
        RuleFor(x => x.Cid).NotEmpty().NotNull();
        RuleFor(x => x.FirstName).NotEmpty().NotNull();
        RuleFor(x => x.LastName).NotEmpty().NotNull();
        RuleFor(x => x.Email).NotEmpty().NotNull();
        RuleFor(x => x.Rating).NotEmpty().NotNull();
        RuleFor(x => x.VisitorFrom).NotEmpty().NotNull();
    }
}
