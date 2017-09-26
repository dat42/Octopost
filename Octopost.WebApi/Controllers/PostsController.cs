namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Services.Posts;

    [Route("api/Posts")]
    public class PostsController : Controller
    {
        private readonly IPostCreationService postCreationService;

        public PostsController(IPostCreationService postCreationService)
        {
            this.postCreationService = postCreationService;
        }

        [HttpPost]
        public IActionResult CreatePost(CreatePostDto createPostDto)
        {
            var id = this.postCreationService.CreatePost(createPostDto);
            return this.Ok();
        }
    }
}
