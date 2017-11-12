namespace Octopost.Validation.Data.Newsletter
{
    using Octopost.Model.Data;
    using Octopost.Model.Settings;
    using Octopost.Services;
    using Octopost.Validation.Common;

    public class PostValidator : AbstractOctopostValidator<Post>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text);
            this.AddRuleForPostTopic(x => x.Topic);
        }
    }
}
