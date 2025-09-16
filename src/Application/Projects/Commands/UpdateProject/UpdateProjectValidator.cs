using Domain.Rules;
using FluentValidation;

namespace Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");

            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .When(x => x.Title != null)
                    .WithMessage("Title cannot be empty when provided.")
                .MaximumLength(ProjectRules.TitleMaxLength)
                .WithMessage($"Title must not exceed {ProjectRules.TitleMaxLength} characters.");

            RuleFor(x => x.Description)
                .MaximumLength(ProjectRules.DescriptionMaxLength)
                .WithMessage($"Description must not exceed {ProjectRules.DescriptionMaxLength} characters.");
        }
    }
}
