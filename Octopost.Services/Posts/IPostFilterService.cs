namespace Octopost.Services.Posts
{
    using Octopost.Model.Dto;
    using System;
    using System.Collections.Generic;

    public interface IPostFilterService
    {
        IEnumerable<PostDto> FilterByTag(string[] tag, int page, int pageSize);

        IEnumerable<PostDto> FilterByDate(DateTime from, DateTime to, int page, int pageSize);

        IEnumerable<PostDto> FilterByVotes(int page, int pageSize);
    }
}
