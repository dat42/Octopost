namespace Octopost.Validation.Common
{
    using FluentValidation;
    using Octopost.Model.Data;
    using Octopost.Model.Interfaces;
    using Octopost.Model.Validation;
    using System;
    using System.Linq.Expressions;

    public static class CommonValidators
    {
        public static void AddRuleForPostText<T>(
            this AbstractOctopostValidator<T> validator,
            Expression<Func<T, string>> textSelector)
        {
            validator.RuleFor(textSelector)
                .MinimumLength(20)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    OctopostEntityName.Post,
                    PropertyName.Post.Text).Code)
                .WithMessage("Text too short");
            validator.RuleFor(textSelector)
                .MaximumLength(140)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.TooShort,
                    OctopostEntityName.Post,
                    PropertyName.Post.Text).Code)
                .WithMessage("Text too long");
        }

        public static void AddRuleForPostTopic<T>(
            this AbstractOctopostValidator<T> validator,
            Expression<Func<T, string>> topicSelector)
        {
            validator.RuleFor(topicSelector)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    OctopostEntityName.Post,
                    PropertyName.Post.Topic).Code)
                .WithMessage("Must be tagged");
        }

        public static void AddRuleForVoteStateString<T>(
            this AbstractOctopostValidator<T> validator,
            Expression<Func<T, string>> property)
        {
            validator.RuleFor(property)
                .Must(x => Enum.TryParse(typeof(VoteState), x, out _))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    OctopostEntityName.Vote,
                    PropertyName.Vote.VoteState).Code)
                .WithMessage("Vote state must be set");
        }

        public static void AddRuleForPaging<T>(
            this AbstractOctopostValidator<T> validator) where T : IPaged
        {
            validator.RuleFor(x => x.PageNumber)
                .Must(x => x >= 0)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    OctopostEntityName.Post,
                    PropertyName.Post.Id).Code)
                .WithMessage("Page number must be non-negative");
            validator.RuleFor(x => x.PageSize)
                .Must(x => x >= 5 && x <= 100)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.OutOfRange,
                    OctopostEntityName.Post,
                    PropertyName.Post.Id).Code)
                .WithMessage("Page size must be between 5 and 100 items");
        }

        public static void AddRuleForMinLength<T>(
            this AbstractOctopostValidator<T> validator,
            Expression<Func<T, string>> property,
            int minimumLength,
            OctopostEntityName entity,
            PropertyName name)
        {
            validator.RuleFor(property)
                .MinimumLength(minimumLength)
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.TooShort,
                    entity,
                    name).Code)
                .WithMessage($"{name.Name} must be at least {minimumLength} characters long");
        }

        public static void AddRuleForNotNullOrEmpty<T>(
            this AbstractOctopostValidator<T> validator,
            Expression<Func<T, string>> property,
            OctopostEntityName entity,
            PropertyName name)
        {
            validator.RuleFor(property)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyDataNullOrEmpty,
                    entity,
                    name).Code)
                .WithMessage($"{name.Name} cannot be null or empty");
        }
    }
}
