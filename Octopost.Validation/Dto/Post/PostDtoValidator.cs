namespace Octopost.Validation.Dto.Newsletter
{
    using Octopost.Validation.Common;
    using Octopost.Model.Dto;

    public class PostDtoValidator : AbstractOctopostValidator<PostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text);
            this.AddRuleForPostTopic(x => x.Topic);
        }
    }
}
