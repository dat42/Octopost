namespace Octopost.Model.Dto
{
    using System;

    public class PostDto
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string Topic { get; set; }

        public long VoteCount { get; set; }

        public DateTime Created { get; set; }
    }
}
