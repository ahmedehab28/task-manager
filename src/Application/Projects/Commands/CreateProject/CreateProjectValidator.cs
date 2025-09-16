using Domain.Rules;
using FluentValidation;

namespace Application.Projects.Commands.CreateProject
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(ProjectRules.TitleMaxLength)
                .WithMessage($"Title must not exceed {ProjectRules.TitleMaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(ProjectRules.DescriptionMaxLength)
                .WithMessage($"Description must not exceed {ProjectRules.DescriptionMaxLength} characters.");
        }
    }
}
