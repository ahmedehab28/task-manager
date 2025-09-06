using FluentValidation;

namespace Application.List.Queries.GetListById
{
    public class GetListByIdValidator : AbstractValidator<GetListByIdQuery>
    {
        public GetListByIdValidator()
        {
            RuleFor(x => x.ListId)
                .NotEmpty().WithMessage("ListId is required.");
        }
    }
}
