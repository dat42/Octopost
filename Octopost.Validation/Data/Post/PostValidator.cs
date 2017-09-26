namespace Octopost.Validation.Data.Newsletter
{
    using Octopost.Validation.Common;
    using Octopost.Model.Data;

    public class PostValidator : AbstractOctopostValidator<Post>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text);
            this.AddRuleForPostTopic(x => x.Topic);
        }
    }
}
