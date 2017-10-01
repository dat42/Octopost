﻿namespace Octopost.Validation.Dto.Newsletter
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class PostDtoValidator : AbstractOctopostValidator<PostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text);
            this.AddRuleForPostTopic(x => x.Topic);
        }
    }
}
