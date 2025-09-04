using FluentValidation;

namespace Application.Projects.Queries.GetProjectById
{
    public class GetProjejctByIdValidator : AbstractValidator<GetProjectByIdQuery>
    {
        public GetProjejctByIdValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}
