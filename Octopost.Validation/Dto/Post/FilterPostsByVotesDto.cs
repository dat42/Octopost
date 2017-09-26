namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Interfaces;

    public class FilterPostsByVotesDto : IPaged
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
