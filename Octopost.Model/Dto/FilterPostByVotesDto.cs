namespace Octopost.Model.Dto
{
    using Octopost.Model.Interfaces;

    public class FilterPostByVotesDto : IPaged
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
