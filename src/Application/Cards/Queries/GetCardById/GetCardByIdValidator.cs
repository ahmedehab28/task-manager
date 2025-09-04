using FluentValidation;

namespace Application.Cards.Queries.GetCardById
{
    public class GetCardByIdValidator : AbstractValidator<GetCardByIdQuery>
    {
        public GetCardByIdValidator()
        {
            RuleFor(x => x.CardId)
                .NotEmpty().WithMessage("CardId is required.");
        }
    }
}
