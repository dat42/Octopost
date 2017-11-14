namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Posts;
    using System;
    using System.Linq;

    [Route("api/Posts")]
    public class FilterPostsController : Controller
    {
        private readonly IPostFilterService postFilterService;

        private readonly IApiResultService apiResultService;

        public FilterPostsController(IPostFilterService postFilterService, IApiResultService apiResultService)
        {
            this.postFilterService = postFilterService;
            this.apiResultService = apiResultService;
        }

        [HttpGet("Tags")]
        public IActionResult FilterByTags(FilterPostByTagDto dto)
        {
            var separated = dto.Tags.Split(',').Select(x => x.Trim()).ToArray();
            var result = this.postFilterService.FilterByTag(separated, dto.PageNumber, dto.PageSize);
            return this.apiResultService.Ok(result);
        }

        [HttpGet("Votes")]
        public IActionResult FilterByVotes(PagedDto dto)
        {
            var result = this.postFilterService.FilterByVotes(dto.PageNumber, dto.PageSize);
            return this.apiResultService.Ok(result);
        }

        [HttpGet("Newest")]
        public IActionResult GetNewest(PagedDto dto)
        {
            var result = this.postFilterService.FilterByDate(DateTime.MinValue, DateTime.MaxValue, dto.PageNumber, dto.PageSize);
            return this.apiResultService.Ok(result);
        }
    }
}
