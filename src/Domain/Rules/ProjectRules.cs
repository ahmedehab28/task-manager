namespace Domain.Rules
{
    public static class ProjectRules
    {
        public const int TitleMaxLength = 50;
        public static readonly string TitleRequiredErrorMessage = "Title cannot be empty.";
        public static readonly string TitleMaxLengthErrorMessage =
            $"Title cannot exceed {TitleMaxLength} characters.";

        public const int DescriptionMaxLength = 500;
        public static readonly string DescriptionMaxLengthErrorMessage =
            $"Description cannot exceed {DescriptionMaxLength} characters.";
    }
}
