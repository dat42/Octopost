namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Interfaces;

    public class FilterPostByTagDto : IPaged
    {
        [FromQuery(Name = "tags")]
        public string Tags { get; set; }

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; }
    }
}
