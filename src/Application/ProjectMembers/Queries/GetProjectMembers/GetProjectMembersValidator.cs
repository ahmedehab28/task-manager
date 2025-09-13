using FluentValidation;

namespace Application.ProjectMembers.Queries.GetProjectMembers
{
    public class GetProjectMembersValidator : AbstractValidator<GetProjectMembersQuery>
    {
        public GetProjectMembersValidator() 
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.");
        }
    }
}
