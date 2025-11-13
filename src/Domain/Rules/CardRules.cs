namespace Domain.Rules
{
    public static class CardRules
    {
        public static readonly string CardIdInvalidErrorMessage = "CardId must be a valid GUID.";

        public const int TitleMaxLength = 50;
        public static readonly string TitleRequiredErrorMessage = "Title cannot be empty.";
        public static readonly string TitleMaxLengthErrorMessage =
            $"Title cannot exceed {TitleMaxLength} characters.";

        public const int DescriptionMaxLength = 500;
        public static readonly string DescriptionMaxLengthErrorMessage =
            $"Description cannot exceed {DescriptionMaxLength} characters.";

        public const int PositionPrecision = 18;
        public const int PositionScale = 4;
        public static readonly string PositionRequiredErrorMessage = "Position cannot be null.";
        public static readonly string PositionNonNegativeErrorMessage = "Position must be a non-negative number.";
        public static readonly string PositionPrecisionErrorMessage = 
            $"Position can have up to {PositionScale} decimal places and " +
            $"no more than {PositionPrecision} digits total.";

        public static readonly string DueDateInPastErrorMessage = "Due date must be in the future.";

        public static readonly string TargetListIdInvalidErrorMessage = "TargetListId must be a valid GUID.";

        public static readonly string MoveCardRequiresPositionErrorMessage = 
            "Position must be provided when TargetListId is specified.";
    }
}
