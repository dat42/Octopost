namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Posts;

    [Route("api/Posts")]
    public class CreatePostsController : Controller
    {
        private readonly IPostCreationService postCreationService;

        private readonly IApiResultService apiResultService;

        public CreatePostsController(IPostCreationService postCreationService, IApiResultService apiResultService)
        {
            this.postCreationService = postCreationService;
            this.apiResultService = apiResultService;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostDto createPostDto)
        {
            var id = this.postCreationService.CreatePost(createPostDto);
            return this.apiResultService.Created(OctopostEntityName.Post, id);
        }
    }
}
