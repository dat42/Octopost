namespace Octopost.Validation.Dto.Newsletter
{
    using Octopost.Validation.Common;
    using Octopost.Model.Dto;

    public class CreatePostDtoValidator : AbstractOctopostValidator<CreatePostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text);
        }
    }
}
