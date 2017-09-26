namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class FilterPostByVotesDtoValidator : AbstractOctopostValidator<FilterPostByVotesDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
        }
    }
}
